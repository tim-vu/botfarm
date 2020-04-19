using System;
using System.Threading;
using System.Threading.Tasks;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm.Bot;
using FORFarm.Application.Mules.Commands.SignalMuleHeartbeat;
using FORFarm.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.UnitTests.Mules.Commands.SignalMuleHeartbeat
{
    public class SignalMuleHeartbeatHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_SignalMuleHeartbeat_NotRunning()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(s => s.IsRunning()).ReturnsAsync(false);
            
            var farmManager = new Mock<IFarmService>();

            var logger = new Mock<ILogger<SignalMuleHeartbeatHandler>>();

            var dateTime = new Mock<IDateTime>();

            var command = new FORFarm.Application.Mules.Commands.SignalMuleHeartbeat.SignalMuleHeartbeat();

            var signalMuleHeartbeatHandler = new SignalMuleHeartbeatHandler(NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var result = await signalMuleHeartbeatHandler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Terminate);
        }

        [Fact]
        public async void Handle_SignalMuleHeartbeat_NonExisting()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(s => s.IsRunning()).ReturnsAsync(true);
            
            var farmManager = new Mock<IFarmService>();

            var logger = new Mock<ILogger<SignalMuleHeartbeatHandler>>();

            var dateTime = new Mock<IDateTime>();
            
            var command = new FORFarm.Application.Mules.Commands.SignalMuleHeartbeat.SignalMuleHeartbeat();
            
            var signalMuleHeartbeatHandler = new SignalMuleHeartbeatHandler(NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var result = await signalMuleHeartbeatHandler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Terminate);
        }

        [Fact]
        public async void Handle_SignalMuleHeartbeat_Existing()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(s => s.IsRunning()).ReturnsAsync(true);

            var logger = new Mock<ILogger<SignalMuleHeartbeatHandler>>();

            var now = DateTime.UtcNow;
            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(d => d.UtcNow).Returns(now);

            var account = BogusData.ValidMuleAccounts.Generate();

            var mule = new Mule
            {
                Account = account,
                Tag = Guid.NewGuid()
            };

            var bot = BogusData.Bots.Generate();

            var muleRequest = new MuleRequest
            {
                Mule = mule,
                Bot = bot
            };

            using (var context = NewContext)
            {
                context.MuleRequests.Add(muleRequest);
                await context.SaveChangesAsync();
            }

            var command = new FORFarm.Application.Mules.Commands.SignalMuleHeartbeat.SignalMuleHeartbeat
            {
                Gold = 500 * 1000,
                Tag = mule.Tag
            };
            
            var signalMuleHeartbeatHandler = new SignalMuleHeartbeatHandler(NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var result = await signalMuleHeartbeatHandler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Continue);
            result.MuleRequests.Should().ContainSingle(bot.DisplayName);

            var uMule = await NewContext.Mules.FindAsync(mule.ID);

            uMule.LastUpdate.Should().Be(now);
            uMule.Gold.Should().Be(command.Gold);
        }
    }
}