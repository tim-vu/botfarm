using System;
using System.IO.Pipes;
using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Bots.Commands.RegisterBot;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm.Bot;
using FORFarm.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.UnitTests.Bots.Commands.RegisterBot
{
    public class RegisterBotHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_RegisterBot_NotRunning()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(f => f.IsRunning()).ReturnsAsync(false);

            var logger = new Mock<ILogger<RegisterBotHandler>>();
            var dateTime = new Mock<IDateTime>();
            
            var registerBotHandler = new RegisterBotHandler(NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var result =
                await registerBotHandler.Handle(new FORFarm.Application.Bots.Commands.RegisterBot.RegisterBot(),
                    CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Terminate);
        }

        [Fact]
        public async void Handle_RegisterBot_NonExistingUsername()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(f => f.IsRunning()).ReturnsAsync(true);

            var logger = new Mock<ILogger<RegisterBotHandler>>();
            var dateTime = new Mock<IDateTime>();
            
            var registerBotHandler = new RegisterBotHandler(NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var result = await registerBotHandler.Handle(
                new FORFarm.Application.Bots.Commands.RegisterBot.RegisterBot {Username = "IDontExist"},
                CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Terminate);
        }

        [Fact]
        public async void Handle_RegisterBot_Valid()
        {
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(f => f.IsRunning()).ReturnsAsync(true);
            
            var logger = new Mock<ILogger<RegisterBotHandler>>();

            var now = DateTime.UtcNow;
            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(d => d.UtcNow).Returns(now);

            var tag = Guid.NewGuid();
            
            var account = BogusData.ValidBotAccounts.Generate();
            var startTime = now.Subtract(TimeSpan.FromSeconds(30));
            
            var bot = new Bot
            {
                Account = account,
                StartTime = startTime,
                LastUpdate = startTime,
            };

            using (var context = NewContext)
            {
                context.Bots.Add(bot);
                await context.SaveChangesAsync();
            }
            
            var registerBotHandler = new RegisterBotHandler(NewContext, logger.Object, dateTime.Object, stateManager.Object);

            var result = await registerBotHandler.Handle(
                new FORFarm.Application.Bots.Commands.RegisterBot.RegisterBot {Tag = tag, Username = account.Username}, CancellationToken.None);

            result.Should().NotBeNull();
            result.Command.Should().Be(Command.Continue);

            var uBot = await NewContext.Bots.FindAsync(bot.ID);

            uBot.Tag.Should().Be(tag);
            uBot.LastUpdate.Should().Be(now);
        }
    }
}