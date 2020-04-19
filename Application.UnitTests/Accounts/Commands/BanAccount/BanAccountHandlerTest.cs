using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Accounts.Commands.BanAccount;
using FORFarm.Application.Common.Exceptions;
using Xunit;

namespace Application.UnitTests.Accounts.Commands.BanAccount
{
    public class BanAccountHandlerTest : TestBase
    {
        [Fact]
        public void Handle_BanAccount_NonExisting()
        {
            var command = new FORFarm.Application.Accounts.Commands.BanAccount.BanAccount()
            {
                ID = 1,
            };
            
            var banAccountHandler = new BanAccountHandler(NewContext);

            Func<Task> act = async () => await banAccountHandler.Handle(command, CancellationToken.None);

            act.Should().Throw<NotFoundException>();
        }
        
        [Fact]
        public async void Handle_BanBot_Existing()
        {
            //setup
            var proxy = Data.Proxies.First();

            var account = BogusData.ValidBotAccounts.Generate();
            account.Proxy = proxy;

            using (var context = NewContext)
            {
                context.Accounts.Add(account);
                await context.SaveChangesAsync();
            }

            var command = new FORFarm.Application.Accounts.Commands.BanAccount.BanAccount
            {
                ID = account.ID
            };

            //act
            var banAccountHandler = new BanAccountHandler(NewContext);
                
            await banAccountHandler.Handle(command, CancellationToken.None);

            //verify
            using (var context = NewContext)
            {
                var uAccount = await context.Accounts.FindAsync(account.ID);
                var uProxy = await context.Proxies.FindAsync(proxy.ID);

                uAccount.Banned.Should().BeTrue();
                uAccount.Proxy.Should().BeNull();

                uProxy.ActiveAccounts.Should().NotContain(uAccount);
                uProxy.BannedBots.Should().Be(1);
            }
        }

        [Fact]
        public async void Handle_BanMule_Existing()
        {
            var proxy = Data.Proxies.First();

            var account = BogusData.ValidMuleAccounts.Generate();
            account.Proxy = proxy;

            using (var context = NewContext)
            {
                context.Accounts.Add(account);
                await context.SaveChangesAsync();
            }

            var command = new FORFarm.Application.Accounts.Commands.BanAccount.BanAccount()
            {
                ID = account.ID
            };

            using (var context = NewContext)
            {
                var banAccountHandler = new BanAccountHandler(context);

                await banAccountHandler.Handle(command, CancellationToken.None);
            }

            using (var context = NewContext)
            {
                var uAccount = await context.Accounts.FindAsync(account.ID);
                var uProxy = await context.Proxies.FindAsync(proxy.ID);

                uAccount.Banned.Should().BeTrue();
                uAccount.Proxy.Should().BeNull();

                uProxy.ActiveAccounts.Should().NotContain(uAccount);
                uProxy.BannedMules.Should().Be(1);
            }
        }
    }
}
