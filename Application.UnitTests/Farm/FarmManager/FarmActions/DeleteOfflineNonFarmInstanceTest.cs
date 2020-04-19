using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;
using FORFarm.Application.Farm.FarmManager.FarmActions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.UnitTests.Farm.FarmManager.FarmActions
{
    public class DeleteOfflineNonFarmInstanceTest : TestBase
    {
        [Fact]
        public async void IsNeeded_Running()
        {
            var instanceService = new Mock<IInstanceService>();
            instanceService.Setup(i => i.IsConnected()).Returns(i => false);
            
            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(s => s.BuildFarmSetup()).ReturnsAsync(FarmSetup.EmptyFarm);
            
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(s => s.IsRunning()).ReturnsAsync(true);

            var instance = BogusData.Bots.Generate();
            using (var context = NewContext)
            {
                context.Bots.Add(instance);
                await context.SaveChangesAsync();
            }

            var action = new DeleteOfflineNonFarmInstance(NewContext, instanceService.Object, farmBuilder.Object, stateManager.Object);

            var result = await action.IsNeeded();

            result.Should().BeTrue();
        }

        [Fact]
        public async void Execute()
        {
            var instanceService = new Mock<IInstanceService>();
            instanceService.Setup(s => s.IsConnected()).Returns(i => false);

            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(f => f.BuildFarmSetup()).ReturnsAsync(FarmSetup.EmptyFarm);

            var stateService = new Mock<IStateService>();

            var instance = BogusData.Bots.Generate();

            using (var context = NewContext)
            {
                context.Instances.Add(instance);
                await context.SaveChangesAsync();
            }

            using (var context = NewContext)
            {
                var deleteOfflineNonFarmInstance = new DeleteOfflineNonFarmInstance(context, instanceService.Object,
                    farmBuilder.Object, stateService.Object);
                await deleteOfflineNonFarmInstance.Execute();
            }

            using (var context = NewContext)
            {
                var instances = await context.Instances.ToListAsync();
                instances.Should().BeEmpty();
            }
        }
    }
}