using System;
using System.Collections.Generic;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;
using FORFarm.Domain.Entities;
using Moq;
using Xunit;

namespace Application.UnitTests.Farm.StateManager
{
    public class StateManagerTest : TestBase
    {
        private static readonly Guid SocketAddress = Guid.Parse("30bff80a-46b2-4468-82f9-087a4204d4ae");

        private static readonly FarmSettings ValidSettings = new FarmSettings
        {
            BotScriptName = "NonEmpty",
            MuleScriptName = "NonEmpty",
            ApiKey = "NonEmpty",
            Hostname = "NonEmpty"
        };
        
        [Fact]
        public async void Start_CanStart_LauncherRunning()
        {
            var now = DateTime.UtcNow;
            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(d => d.UtcNow).Returns(now);

            var settingsManager = new Mock<ISettingsService>();
            settingsManager.Setup(s => s.AreSettingsReady()).ReturnsAsync(true);

            using (var context = NewContext)
            {
                var settings = Clone(ValidSettings);
                settings.ID = 1;
                context.Update(settings);
                await context.SaveChangesAsync();
            }
            
            var launcher = new Mock<ILauncher>();
            launcher.Setup(l => l.Hostname).Returns(ValidSettings.Hostname);
            launcher.Setup(l => l.SocketAddress).Returns(SocketAddress);

            var farm = new FarmSetup(new List<Account>{new Account()}, new List<Account>());
            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(f => f.BuildFarmSetup()).ReturnsAsync(farm);
            
            var clientHandler = new Mock<IClientHandler>();
            clientHandler.Setup(c => c.GetLaunchers(It.IsAny<string>())).ReturnsAsync(new List<ILauncher> {launcher.Object});
            
            using (var context = NewContext)
            {
                var stateManager = new FORFarm.Application.Farm.StateService(context, dateTime.Object, settingsManager.Object, farmBuilder.Object, clientHandler.Object);
                await stateManager.Start();
            }

            var state = await NewContext.FarmStates.FindAsync(1);

            settingsManager.Verify(s => s.ActivateFarmSettings());
            
            state.Running.Should().BeTrue();
            state.Start.Should().Be(now);
            state.SocketAddress.Should().Be(SocketAddress);
        }

        [Fact]
        public async void Stop()
        {
            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(d => d.UtcNow).Returns(DateTime.UtcNow);

            var settingsManager = new Mock<ISettingsService>();
            var farmBuilder = new Mock<IFarmBuilder>();
            var clientHandler = new Mock<IClientHandler>();

            using (var context = NewContext)
            {
                var stateManager = new FORFarm.Application.Farm.StateService(context, dateTime.Object, settingsManager.Object, farmBuilder.Object, clientHandler.Object);
                await stateManager.Start();
            }

            using (var context = NewContext)
            {
                var stateManager = new FORFarm.Application.Farm.StateService(context, dateTime.Object, settingsManager.Object, farmBuilder.Object, clientHandler.Object);
                await stateManager.Stop();
            }

            var state = await NewContext.FarmStates.FindAsync(1);

            state.Running.Should().BeFalse();
        }
    }
}