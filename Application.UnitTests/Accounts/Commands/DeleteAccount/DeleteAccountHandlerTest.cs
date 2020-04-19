using System.Linq;
using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Accounts.Commands.DeleteAccount;
using Xunit;

namespace Application.UnitTests.Accounts.Commands.DeleteAccount
{
    public class DeleteAccountHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_DeleteAccount()
        {
            var instance = BogusData.Bots.Generate();
            var account = instance.Account;
            var proxy = BogusData.Proxies.Generate();
            account.Proxy = proxy;

            using (var context = NewContext)
            {
                context.Instances.Add(instance);
                await context.SaveChangesAsync();
            }

            var deleteAccount = new FORFarm.Application.Accounts.Commands.DeleteAccount.DeleteAccount {ID = account.ID};
            
            var deleteAccountHandler = new DeleteAccountHandler(NewContext);

            await deleteAccountHandler.Handle(deleteAccount, CancellationToken.None);

            using (var context = NewContext)
            {
                context.Instances.ToList().Should().BeEmpty();
                context.Accounts.ToList().Should().BeEmpty();

                var uProxy = context.Proxies.Find(proxy.ID);

                uProxy.ActiveAccounts.Should().BeEmpty();
            }
        }
    }
}