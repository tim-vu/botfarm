using System;
using System.Threading;
using Application.UnitTests.Common;
using Bogus;
using FluentAssertions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm.Bot;
using FORFarm.Application.Mules.Commands.RegisterMule;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.UnitTests.Mules.Commands.RegisterMule
{
    public class RegisterMuleHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_RegisterMule_NotRunning()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(s => s.IsRunning()).ReturnsAsync(false);
            
            var farmManager = new Mock<IFarmService>();

            var logger = new Mock<ILogger<RegisterMuleHandler>>();
            var dateTime = new Mock<IDateTime>();
            
            var registerMuleHandler = new RegisterMuleHandler(farmManager.Object, NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var result =
                await registerMuleHandler.Handle(new FORFarm.Application.Mules.Commands.RegisterMule.RegisterMule(),
                    CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Terminate);
            result.Position.Should().BeNull();
        }

        [Fact]
        public async void Handle_RegisterMule_NonExisting()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(s => s.IsRunning()).ReturnsAsync(true);
            
            var farmManager = new Mock<IFarmService>();

            var logger = new Mock<ILogger<RegisterMuleHandler>>();
            var dateTime = new Mock<IDateTime>();
            
            var registerMuleHandler = new RegisterMuleHandler(farmManager.Object, NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var command = new FORFarm.Application.Mules.Commands.RegisterMule.RegisterMule()
            {
                Username = "IDontExist"
            };

            var result = await registerMuleHandler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Terminate);
            result.Position.Should().BeNull();
        }

        [Fact]
        public async void Handle_RegisterMule_Existing()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(s => s.IsRunning()).ReturnsAsync(true);
            
            var farmManager = new Mock<IFarmService>();
            var position = Data.Position;
            farmManager.Setup(f => f.GetMulePosition()).ReturnsAsync(position);
            
            var logger = new Mock<ILogger<RegisterMuleHandler>>();

            var now = DateTime.UtcNow;
            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(d => d.UtcNow).Returns(now);

            var account = BogusData.ValidMuleAccounts.Generate();

            var startTime = now.Subtract(TimeSpan.FromSeconds(30));
            var mule = new Mule
            {
                Account = account,
                StartTime = startTime,
                LastUpdate = startTime
            };

            using (var context = NewContext)
            {
                context.Mules.Add(mule);

                await context.SaveChangesAsync();
            }

            var command = new FORFarm.Application.Mules.Commands.RegisterMule.RegisterMule()
            {
                Tag = Guid.NewGuid(),
                Username = account.Username,
                DisplayName = "DarudeSandstorm"
            };
            
            var registerMuleHandler = new RegisterMuleHandler(farmManager.Object, NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var result = await registerMuleHandler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Continue);

            var uMule = await NewContext.Mules
                .Include(m => m.Position)
                .FirstOrDefaultAsync(m => m.ID == mule.ID);

            uMule.Position.Should().BeEquivalentTo(position);
            uMule.DisplayName.Should().Be(command.DisplayName);
            uMule.Tag.Should().Be(command.Tag);
            uMule.LastUpdate.Should().Be(now);
        }
    }
}