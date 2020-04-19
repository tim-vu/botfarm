using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Accounts.Commands.UpdateGameDetails;
using FORFarm.Application.Common.Exceptions;
using FORFarm.Application.Common.Interfaces;
using Moq;
using Xunit;

namespace Application.UnitTests.Accounts.Commands.UpdateDetails
{
    public class UpdateGameDetailsHandlerTest : TestBase
    {
        [Fact]
        public void Handle_UpdateGameDetails_NonExisting()
        {
            var dateTime = new Mock<IDateTime>();
            
            var command = new UpdateGameDetails()
            {
                ID = 1,
            };

            var updateGameDetailsHandler = new UpdateGameDetailsHandler(NewContext, dateTime.Object);

            Func<Task> act = async () => await updateGameDetailsHandler.Handle(command, CancellationToken.None);

            act.Should().Throw<NotFoundException>();
        }
        
        [Fact]
        public async void Handle_UpdateGameDetails_Existing()
        {
            var account = BogusData.MixedAccounts.Generate();
            var skills = BogusData.GenerateRandomSkills();
            var skillDictionary = skills.ToDictionary(s => s.Type, s => s.Level);

            using(var context = NewContext)
            {
                context.Accounts.Add(account);
                await context.SaveChangesAsync();
            }

            var now = DateTime.UtcNow;
            var datetime = new Mock<IDateTime>();
            datetime.Setup(d => d.UtcNow).Returns(now);

            const int daysRemaining = 5;
            var command = new UpdateGameDetails
            {
                ID = account.ID,
                MembershipDaysRemaining = daysRemaining,
                Skills = skillDictionary
            };

            var updateGameDetailsHandler = new UpdateGameDetailsHandler(NewContext, datetime.Object);

            await updateGameDetailsHandler.Handle(command, CancellationToken.None);

            var newDate = now.AddDays(daysRemaining);

            var uAccount = await NewContext.Accounts.FindAsync(account.ID);

            uAccount.MemberExpirationDate.Should().Be(newDate);
            uAccount.Skills.Should().BeEquivalentTo(skills);
        }
    }
}
