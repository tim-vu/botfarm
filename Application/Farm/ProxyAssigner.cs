using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Farm
{
    public static class ProxyAssigner
    {
        public static async Task<bool> AssignProxies(IFarmContext context, FarmSetup farmSetup, FarmSettings settings)
        {
            var neededBotProxySpots = farmSetup.Bots.Count(a => a.Proxy == null);
            var neededMuleProxySpots = farmSetup.Mules.Count(a => a.Proxy == null);

            var neededSpots = neededBotProxySpots + neededMuleProxySpots;

            if (neededSpots == 0)
                return true;

            var candidateBotProxies = await context.Proxies
                .Include(p => p.ActiveAccounts)
                .Where(HasNoMules)
                .Where(p => p.ActiveAccounts.Count < settings.ConcurrentAccountsPerProxy)
                .Where(p => p.BannedMules + p.BannedBots + p.ActiveAccounts.Count < settings.MaxAccountsPerProxy)
                .Take(neededBotProxySpots).ToListAsync();

            var remainingSpotsToAssign = neededBotProxySpots;
            var takenProxyIds = new HashSet<int>();

            foreach (var proxy in candidateBotProxies)
            {
                if (remainingSpotsToAssign == 0)
                    break;

                remainingSpotsToAssign -= GetRemainingSpots(proxy, settings.ConcurrentAccountsPerProxy, settings.MaxActiveBots);
                takenProxyIds.Add(proxy.ID);
            }

            if (remainingSpotsToAssign > 0)
                return false;

            var remainingProxies = candidateBotProxies
                .Where(p => !takenProxyIds.Contains(p.ID))
                .Where(HasNoBots.Compile())
                .Where(HasSpotRemaining(settings.ConcurrentAccountsPerProxy, settings.MaxAccountsPerProxy).Compile())
                .ToList();
            
            var remainingSpots = GetRemainingSpots(remainingProxies, settings.ConcurrentAccountsPerProxy,
                settings.MaxAccountsPerProxy);

            var candidateMuleProxies = await context.Proxies
                .Include(p => p.ActiveAccounts)
                .Where(HasNoBots)
                .Where(p => !takenProxyIds.Contains(p.ID))
                .Take(neededMuleProxySpots - remainingSpots).ToListAsync();

            candidateBotProxies.RemoveAll(p => !takenProxyIds.Contains(p.ID));
            candidateMuleProxies.AddRange(remainingProxies);

            remainingSpotsToAssign = neededMuleProxySpots;

            foreach (var proxy in candidateMuleProxies)
            {
                if (remainingSpotsToAssign == 0)
                    break;

                remainingSpotsToAssign -= GetRemainingSpots(proxy, settings.ConcurrentAccountsPerProxy,
                    settings.MaxAccountsPerProxy);
            }

            if (remainingSpotsToAssign > 0)
                return false;

            foreach (var acc in farmSetup.Bots.Where(a => a.Proxy == null))
            {
                var proxy = candidateBotProxies.First(
                    HasSpotRemaining(settings.ConcurrentAccountsPerProxy, settings.MaxAccountsPerProxy).Compile());

                acc.Proxy = proxy;
                proxy.ActiveAccounts.Add(acc);
            }

            foreach (var acc in farmSetup.Mules.Where(a => a.Proxy == null))
            {
                var proxy = candidateMuleProxies.First(
                    HasSpotRemaining(settings.ConcurrentAccountsPerProxy, settings.MaxAccountsPerProxy).Compile());

                acc.Proxy = proxy;
                proxy.ActiveAccounts.Add(acc);
            }

            await context.SaveChangesAsync();

            return true;
        }
        
        private static Expression<Func<Proxy, bool>> HasSpotRemaining(int maxConcurrentBots, int maxTotalBots)
        {
            return proxy => GetRemainingSpots(proxy, maxConcurrentBots, maxTotalBots) > 0;
        }

        private static int GetRemainingSpots(Proxy proxy, int maxConcurrentBots, int maxTotalBots)
        {
            if (proxy.ActiveAccounts.Count >= maxConcurrentBots)
                return 0;

            return Math.Min(maxConcurrentBots - proxy.ActiveAccounts.Count, maxTotalBots - proxy.ActiveAccounts.Count - proxy.PreviousAccounts);
        }

        private static int GetRemainingSpots(IEnumerable<Proxy> proxies, int maxConcurrentBots,
            int maxTotalBots)
        {
            return proxies.Sum(p => GetRemainingSpots(p, maxConcurrentBots, maxTotalBots));
        }
        
        private static readonly Expression<Func<Proxy, bool>> HasNoMules = proxy =>
            proxy.ActiveAccounts.Count(a => a.Mule) == 0 && proxy.BannedMules == 0;

        private static readonly Expression<Func<Proxy, bool>> HasNoBots = proxy =>
            proxy.ActiveAccounts.Count(a => !a.Mule) == 0 || proxy.BannedBots == 0;
    }
}