using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Farm
{
    public static class AccountAssigner
    {
        public static async Task<FarmSetup> AssignAccounts(IFarmContext context, FarmSettings settings)
        {
            var possibleMules = await context.Accounts
                .Include(a => a.Proxy)
                .Where(a => a.Mule)
                .Where(IsUsable)
                .Take(settings.MaxActiveMules).ToListAsync();

            if (possibleMules.Count < settings.MinActiveMules)
                return new FarmSetup();

            var possibleBots = await context.Accounts
                .Include(a => a.Proxy)
                .Where(a => !a.Mule)
                .Where(IsUsable)
                .Take(settings.MaxActiveBots)
                .ToListAsync();

            if (possibleBots.Count == 0)
                return new FarmSetup();

            return new FarmSetup(possibleBots, possibleMules);
        }
        
        private static readonly Expression<Func<Account, bool>> IsUsable = account =>
            account.MemberExpirationDate > DateTime.UtcNow && !account.Banned;
        
    }
}