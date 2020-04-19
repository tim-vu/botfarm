using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;
using FORFarm.Application.Farm.FarmManager.FarmActions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.UnitTests.Farm.FarmManager.FarmActions
{
    public class StopFarmWhenEmptyTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void IsNeeded(bool emptyFarm)
        {
            var farm = emptyFarm
                ? new FarmSetup()
                : new FarmSetup(BogusData.ValidBotAccounts.Generate(6), BogusData.ValidMuleAccounts.Generate(2));

            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(f => f.BuildFarmSetup()).ReturnsAsync(farm);

            var stateManager = new Mock<IStateService>();
            var logger = new Mock<ILogger<StopFarmWhenEmpty>>();
            
            var stopFarmWhenEmpty = new StopFarmWhenEmpty(farmBuilder.Object, stateManager.Object, logger.Object);

            var result = await stopFarmWhenEmpty.IsNeeded();

            result.Should().Be(emptyFarm);
        }

        [Fact]
        public async void Execute()
        {
            var farmBuilder = new Mock<IFarmBuilder>();
            var stateManager = new Mock<IStateService>();
            var logger = new Mock<ILogger<StopFarmWhenEmpty>>();
            
            var stopFarmWhenEmpty = new StopFarmWhenEmpty(farmBuilder.Object, stateManager.Object, logger.Object);

            await stopFarmWhenEmpty.Execute();
            
            stateManager.Verify(s => s.Stop());
        }
    }
}