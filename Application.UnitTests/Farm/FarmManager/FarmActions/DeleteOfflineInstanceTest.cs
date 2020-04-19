using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Farm.FarmManager.FarmActions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.UnitTests.Farm.FarmManager.FarmActions
{
    public class DeleteOfflineInstanceTest : TestBase
    {
        [Fact]
        public async void Execute()
        {
            var bots = BogusData.Bots.Generate();
            var stateManager = new Mock<IStateService>();
            var logger = new Mock<ILogger<DeleteOfflineInstance>>();
            
            var instance = new Mock<IInstanceService>();
            instance.Setup(i => i.IsConnected()).Returns(i => false);
            
            using (var context = NewContext)
            {
                context.AddRange(bots);
                await context.SaveChangesAsync();
            }
            
            var deleteOfflineInstance = new DeleteOfflineInstance(NewContext, stateManager.Object, logger.Object, instance.Object);

            await deleteOfflineInstance.Execute();

            using (var context = NewContext)
            {
                var instances = await context.Instances.ToListAsync();
                instances.Should().BeEmpty();
            }
        }
    }
}