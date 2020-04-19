using System;
using System.Runtime.CompilerServices;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;
using FORFarm.Domain.Entities;
using Moq;
using Xunit;

namespace Application.UnitTests.Farm.InstanceService
{
    public class InstanceServiceTest : TestBase
    {
        private readonly IInstanceService _instanceService;
        private readonly DateTime _now;

        private TimeSpan LaunchTimeout => FORFarm.Application.Farm.InstanceService.LaunchTimeout;
        private TimeSpan HeartbeatTimeout => FORFarm.Application.Farm.InstanceService.HeartbeatTimeout;

        public InstanceServiceTest()
        {
            _now = DateTime.UtcNow;

            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(d => d.UtcNow).Returns(_now);

            _instanceService = new FORFarm.Application.Farm.InstanceService(
                dateTime.Object,
                new Mock<IClientHandler>().Object,
                new Mock<IStateService>().Object, 
                Mapper, 
                NewContext);
        }
        
        [Fact]
        public void IsConnected()
        {
            var instance = BogusData.Bots.Generate();
            instance.StartTime = _now.Subtract(LaunchTimeout);
            instance.LastUpdate = _now.Subtract(HeartbeatTimeout.Subtract(TimeSpan.FromSeconds(5)));
            
            var isConnected = _instanceService.IsConnected().Compile();

            var result = isConnected(instance);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsConnected_NoUpdate()
        {
            var instance = BogusData.Bots.Generate();
            instance.StartTime = _now.Subtract(LaunchTimeout.Add(TimeSpan.FromSeconds(5)));
            instance.LastUpdate = instance.StartTime;

            var isConnected = _instanceService.IsConnected().Compile();

            var result = isConnected(instance);
            
            result.Should().BeFalse();
        }

        [Fact]
        public void IsConnected_HeartbeatTimeout()
        {
            var instance = BogusData.Bots.Generate();
            instance.StartTime = _now.Subtract(LaunchTimeout.Add(LaunchTimeout));
            instance.LastUpdate = _now.Subtract(HeartbeatTimeout.Add(TimeSpan.FromSeconds(5)));

            var isConnected = _instanceService.IsConnected().Compile();

            var result = isConnected(instance);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async void LaunchInstance(bool startClient, bool mule)
        {
            Instance instance;

            if (mule)
                instance = BogusData.Mules.Generate();
            else
                instance = BogusData.Bots.Generate();
                
            var settings = new FarmSettings
            {
                ID = 2,
                BotScriptName = "BotScript",
                MuleScriptName = "MuleScrip",
                ApiKey = "JFDKSLFDSJKL7u894312709jfdksl"
            };

            using (var context = NewContext)
            {
                context.FarmSettingsTable.Update(settings);
                await context.SaveChangesAsync();
            }
            
            var state = new FarmState
            {
                SocketAddress = Guid.NewGuid()
            };
            
            var dateTime = new Mock<IDateTime>();
            
            var stateService = new Mock<IStateService>();
            stateService.Setup(s => s.GetFarmState()).ReturnsAsync(state);
            
            var clientHandler = new Mock<IClientHandler>();
            clientHandler.Setup(c => c.StartClient(It.IsAny<Guid>(), It.IsAny<ClientStartArgs>(), It.IsAny<string>()))
                .ReturnsAsync(startClient);

            var instanceService = new FORFarm.Application.Farm.InstanceService(dateTime.Object, clientHandler.Object, stateService.Object, Mapper, NewContext);

            var result = await instanceService.LaunchInstance(instance);

            result.Should().Be(startClient);
            
            clientHandler.Verify(c => c.StartClient(
                It.Is<Guid>(s => s == state.SocketAddress), 
                It.Is<ClientStartArgs>(
                    args => args.Username == instance.Account.Username && 
                            args.Password == instance.Account.Password && 
                            args.ScriptName == (mule ? settings.MuleScriptName : settings.BotScriptName)
                            ), It.Is<string>(s => s == settings.ApiKey)));
        }
    }
}