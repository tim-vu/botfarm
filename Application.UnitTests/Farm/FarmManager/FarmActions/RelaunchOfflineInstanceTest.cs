using System;
using System.Collections.Generic;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;
using FORFarm.Application.Farm.FarmManager.FarmActions;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Application.UnitTests.Farm.FarmManager.FarmActions
{
    public class RelaunchOfflineInstanceTest : TestBase
    {
        [Fact]
        public async void IsNeeded()
        {
            var instance = BogusData.Bots.Generate();

            using (var context = NewContext)
            {
                context.Instances.Add(instance);
                await context.SaveChangesAsync();
            }
            
            var dateTime = new Mock<IDateTime>();
            
            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(f => f.BuildFarmSetup())
                .ReturnsAsync(new FarmSetup(new List<Account> {instance.Account}, new List<Account>()));
            
            var instanceService = new Mock<IInstanceService>();
            instanceService.Setup(i => i.IsConnected()).Returns(i => false);

            bool result;
            using (var context = NewContext)
            {
                var relaunchOfflineInstance = new RelaunchOfflineInstance(context, dateTime.Object, farmBuilder.Object, instanceService.Object);

                result = await relaunchOfflineInstance.IsNeeded();
            }

            result.Should().BeTrue();
        }

        [Fact]
        public async void Execute_LaunchInstanceFails()
        {
            var instance = BogusData.Bots.Generate();

            using (var context = NewContext)
            {
                context.Instances.Add(instance);
                await context.SaveChangesAsync();
            }
            
            var dateTime = new Mock<IDateTime>();

            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(f => f.BuildFarmSetup())
                .ReturnsAsync(new FarmSetup(new List<Account> {instance.Account}, new List<Account>()));

            var instanceService = new Mock<IInstanceService>();
            instanceService.Setup(i => i.IsConnected()).Returns(i => false);
            instanceService.Setup(i => i.LaunchInstance(It.IsAny<Instance>())).ReturnsAsync(false);

            using (var context = NewContext)
            {
                var relaunchOfflineInstance = new RelaunchOfflineInstance(context, dateTime.Object, farmBuilder.Object, instanceService.Object);

                await relaunchOfflineInstance.Execute();
            }

            var instances = await NewContext.Instances.Include(i => i.Account).ToListAsync();

            instances.Should().ContainSingle();
            instances.Should().ContainEquivalentOf(instance);
        }

        [Fact]
        public async void Execute_LaunchInstanceSucceeds()
        {
            var instance = BogusData.Bots.Generate();

            using (var context = NewContext)
            {
                context.Instances.Add(instance);
                await context.SaveChangesAsync();
            }

            var now = DateTime.UtcNow;
            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(d => d.UtcNow).Returns(now);

            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(f => f.BuildFarmSetup())
                .ReturnsAsync(new FarmSetup(new List<Account>() {instance.Account}, new List<Account>()));

            var instanceService = new Mock<IInstanceService>();
            instanceService.Setup(i => i.IsConnected()).Returns(i => false);
            instanceService.Setup(i => i.LaunchInstance(It.IsAny<Instance>())).ReturnsAsync(true);

            using (var context = NewContext)
            {
                var relaunchOfflineInstance = new RelaunchOfflineInstance(context, dateTime.Object, farmBuilder.Object, instanceService.Object);

                await relaunchOfflineInstance.Execute();
            }

            var instances = await NewContext.Instances.Include(i => i.Account).ToListAsync();

            instances.Should().ContainSingle();
            
            var uInstance = instances[0];
            uInstance.Should().NotBeNull();
            uInstance.Account.Should().BeEquivalentTo(instance.Account);
            uInstance.Tag.Should().Be(Guid.Empty);
            uInstance.StartTime.Should().Be(now);
            uInstance.LastUpdate.Should().Be(now);
        }
    }
}