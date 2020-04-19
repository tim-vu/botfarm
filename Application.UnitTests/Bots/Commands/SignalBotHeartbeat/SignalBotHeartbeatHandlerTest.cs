using System;
using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Bots.Commands.SignalHeartbeat;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm.Bot;
using FORFarm.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.UnitTests.Bots.Commands.SignalBotHeartbeat
{
    public class SignalBotHeartbeatHandlerTest : TestBase
    {
        
        
        [Fact]
        public async void Handle_SignalBotHeartbeat_NonExistingTag()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(f => f.IsRunning()).ReturnsAsync(true);
            
            var logger = new Mock<ILogger<SignalBotHeartbeatHandler>>();
            var dateTime = new Mock<IDateTime>();
            
            var tag = Guid.NewGuid();
            
            var signalBotHeartbeatHandler = new SignalBotHeartbeatHandler(NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var result = await signalBotHeartbeatHandler.Handle(
                new FORFarm.Application.Bots.Commands.SignalHeartbeat.SignalBotHeartbeat {Tag = tag},
                CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Terminate);
        }

        [Fact]
        public async void Handler_SignalBotHeartbeat_Valid()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(f => f.IsRunning()).ReturnsAsync(true);
            
            var logger = new Mock<ILogger<SignalBotHeartbeatHandler>>();

            var now = DateTime.UtcNow;
            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(d => d.UtcNow).Returns(now);
            
            var tag = Guid.NewGuid();
            var startTime = now.Subtract(TimeSpan.FromSeconds(30));
            
            var account = BogusData.ValidBotAccounts.Generate();

            var bot = new Bot
            {
                Account = account,
                Tag = tag,
                StartTime = startTime,
                LastUpdate = startTime
            };

            using (var context = NewContext)
            {
                context.Bots.Add(bot);
                await context.SaveChangesAsync();
            }

            var signalBotHeartbeatHandler = new SignalBotHeartbeatHandler(NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var result = await signalBotHeartbeatHandler.Handle(
                new FORFarm.Application.Bots.Commands.SignalHeartbeat.SignalBotHeartbeat
                    {Tag = tag}, CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Continue);

            var uBot = await NewContext.Bots.FindAsync(bot.ID);

            uBot.LastUpdate.Should().Be(now);
        }
    }
}