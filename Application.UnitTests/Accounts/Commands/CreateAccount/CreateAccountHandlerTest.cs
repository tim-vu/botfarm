using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Accounts.Commands.CreateAccount;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.UnitTests.Accounts.Commands.CreateAccount
{
    public class CreateAccountHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_CreateAccount_NewUsername()
        {
            const int remainingDays = 5;

            var command = new FORFarm.Application.Accounts.Commands.CreateAccount.CreateAccount
            {
                Username = "User123",
                Password = "SuperSecretPassword",
                RemainingMembershipDays = remainingDays
            };

            var now = DateTime.UtcNow;

            var moq = new Mock<IDateTime>();
            moq.Setup(d => d.UtcNow).Returns(now);

            var createAccountHandler = new CreateAccountHandler(NewContext, moq.Object, Mapper);

            await createAccountHandler.Handle(command, CancellationToken.None);

            var accountVm = await NewContext.Accounts.FirstOrDefaultAsync(a => a.Username == command.Username);
            accountVm.Username.Should().Be(command.Username);
            accountVm.Password.Should().Be(command.Password);
            accountVm.MemberExpirationDate.Should().Be(now.AddDays(remainingDays));
        }
    }
}
