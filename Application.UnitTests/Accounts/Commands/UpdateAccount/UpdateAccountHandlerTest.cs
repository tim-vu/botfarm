using System;
using System.Threading;
using System.Threading.Tasks;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Accounts.Commands.UpdateAccount;
using FORFarm.Application.Common.Exceptions;
using FORFarm.Domain.Entities;
using Xunit;

namespace Application.UnitTests.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountHandlerTest : TestBase
    {
        /*
        [Fact]
        public async void Handle_UpdateAccount_NonExisting()
        {
            var command = new FORFarm.Application.Accounts.Commands.UpdateAccount.UpdateAccount()
            {
                ID = 1,
            };
            
            var updateAccountHandler = new UpdateAccountHandler(Context, Mapper);

            Func<Task> act = async () => await updateAccountHandler.Handle(command, CancellationToken.None);

            act.Should().Throw<NotFoundException>();
        }
        */
        
        [Fact]
        public async void Handle_UpdateAccount_Existing()
        {
            var account = new Account
            {
                Username = "User123",
                Password = "SuperSecretPassword",
                MemberExpirationDate = DateTime.UtcNow.AddDays(5)
            };

            using (var context = NewContext)
            {
                context.Accounts.Add(account);
                await context.SaveChangesAsync();
            }

            var command = new FORFarm.Application.Accounts.Commands.UpdateAccount.UpdateAccount
            {
                ID = account.ID,
                Mule = true,
                Password = "SuperSecretPassword2"
            };

            using (var context = NewContext)
            {
                var updateAccountHandler = new UpdateAccountHandler(context);

                await updateAccountHandler.Handle(command, CancellationToken.None);
            }

            using (var context = NewContext)
            {
                var uAccount = await context.Accounts.FindAsync(account.ID);

                uAccount.Password.Should().Be(command.Password);
                uAccount.Mule.Should().Be(command.Mule);
            }
        }
    }
}
