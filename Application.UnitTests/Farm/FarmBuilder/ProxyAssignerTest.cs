using System;
using System.Linq;
using System.Threading.Tasks;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Models.Farm;
using FORFarm.Application.Farm;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Xunit;

namespace Application.UnitTests.Farm.FarmBuilder
{
    public class ProxyAssignerTest : FarmBuilderTestBase
    {
        [Fact]
        public async void AssignProxies_ValidNoneAssigned()
        {
            //setup
            FarmSetup farmSetup;
            using (var context = NewContext)
            {
                farmSetup = await CreateValidFarmSetup(context);

                int botProxyCount = (int)Math.Ceiling(farmSetup.Bots.Count / (float)ConcurrentAccountsPerProxy);
                int muleProxyCount = (int)Math.Ceiling(farmSetup.Mules.Count / (float)ConcurrentAccountsPerProxy);
            
                var sProxies = BogusData.Proxies.Generate(botProxyCount + muleProxyCount);
            
                context.Proxies.AddRange(sProxies);

                await context.SaveChangesAsync();
            }
            
            //act
            var result = await ProxyAssigner.AssignProxies(NewContext, farmSetup, Settings);

            //verify
            result.Should().BeTrue();

            var accounts = await NewContext.Accounts
                .Include(a => a.Proxy)
                .ToListAsync();
            var proxies = await NewContext.Proxies
                .Include(p => p.ActiveAccounts)
                .ToListAsync();

            accounts.Should().OnlyContain(a => a.Proxy != null);

            proxies.Should().OnlyContain(p => p.ActiveAccounts.Count > 0);
            proxies.Should().OnlyContain(p => p.ActiveAccounts.All(a => a.Mule) || p.ActiveAccounts.All(a => !a.Mule));
        }

        [Fact]
        public async void AssignProxies_ValidAllAssigned()
        {
            FarmSetup farmSetup;
            using (var context = NewContext)
            {
                farmSetup = await CreateValidFarmSetup(context);
            
                farmSetup.Accounts.ForEach(a => a.Proxy = BogusData.Proxies.Generate());

                await context.SaveChangesAsync();
            }

            var result = await ProxyAssigner.AssignProxies(NewContext, farmSetup, Settings);

            result.Should().BeTrue();

            var proxies = await NewContext.Proxies.Include(p => p.ActiveAccounts).ToListAsync();

            proxies.Should().OnlyContain(p => p.ActiveAccounts.Count == 1);
        }

        [Fact]
        public async void AssignProxies_NotEnoughBotProxies()
        {
            FarmSetup farmSetup;
            using (var context = NewContext)
            {
                farmSetup = await CreateValidFarmSetup(context);

                var botProxyCount = (int) Math.Ceiling(farmSetup.Bots.Count / (float) ConcurrentAccountsPerProxy) - 1;

                var proxies = BogusData.Proxies.Generate(botProxyCount);


                context.Proxies.AddRange(proxies);

                await context.SaveChangesAsync();
            }

            var result = await ProxyAssigner.AssignProxies(NewContext, farmSetup, Settings);

            result.Should().BeFalse();
        }

        [Fact]
        public async void AssignProxies_NotEnoughMuleProxies()
        {
            FarmSetup farmSetup;
            using (var context = NewContext)
            {
                farmSetup = await CreateValidFarmSetup(context);

                var botProxyCount = (int) Math.Ceiling(farmSetup.Bots.Count / (float) ConcurrentAccountsPerProxy);

                var proxies = BogusData.Proxies.Generate(botProxyCount);
                
                context.Proxies.AddRange(proxies);

                await context.SaveChangesAsync();
            }

            var result = await ProxyAssigner.AssignProxies(NewContext, farmSetup, Settings);

            result.Should().BeFalse();
        }

        private async Task<FarmSetup> CreateValidFarmSetup(ForFarmDbContext context)
        {
            var sBots = BogusData.ValidBotAccounts.Generate(MaxActiveBots);
            var sMules = BogusData.ValidMuleAccounts.Generate(MaxActiveMules);
            
            context.Accounts.AddRange(sBots);
            context.Accounts.AddRange(sMules);

            await context.SaveChangesAsync();

            return await AccountAssigner.AssignAccounts(context, Settings);

        }
    }
    
 
}