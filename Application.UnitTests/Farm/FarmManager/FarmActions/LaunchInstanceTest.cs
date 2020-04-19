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
    public class LaunchInstanceTest : TestBase
    {
        [Fact]
        public async void IsNeeded()
        {
            var toLaunchAccount = BogusData.ValidBotAccounts.Generate();
            
            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(f => f.BuildFarmSetup()).ReturnsAsync(new FarmSetup(new List<Account> {toLaunchAccount}, new List<Account>()));

            var instanceService = new Mock<IInstanceService>();
            var dateTime = new Mock<IDateTime>();
            
            var launchInstance = new LaunchInstance(farmBuilder.Object, NewContext, instanceService.Object, dateTime.Object);

            var result = await launchInstance.IsNeeded();

            result.Should().BeTrue();
        }

        [Fact]
        public async void Execute_LaunchInstanceFails()
        {
            var toLaunchAccount = BogusData.ValidBotAccounts.Generate();

            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(f => f.BuildFarmSetup())
                .ReturnsAsync(new FarmSetup(new List<Account> {toLaunchAccount}, new List<Account>()));

            var instanceService = new Mock<IInstanceService>();
            instanceService.Setup(i => i.LaunchInstance(It.IsAny<Instance>())).ReturnsAsync(false);
            
            var dateTime = new Mock<IDateTime>();

            var launchInstance = new LaunchInstance(farmBuilder.Object, NewContext, instanceService.Object, dateTime.Object);

            await launchInstance.Execute();

            var instances = await NewContext.Instances.ToListAsync();

            instances.Should().BeEmpty();
        }

        [Fact]
        public async void Execute_LaunchInstanceSucceeds()
        {
            var toLaunchAccount = BogusData.ValidBotAccounts.Generate();

            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(f => f.BuildFarmSetup())
                .ReturnsAsync(new FarmSetup(new List<Account> {toLaunchAccount}, new List<Account>()));

            var now = DateTime.UtcNow;
            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(d => d.UtcNow).Returns(now);

            var instanceService = new Mock<IInstanceService>();
            instanceService.Setup(i => i.LaunchInstance(It.IsAny<Instance>())).ReturnsAsync(true);

            using (var context = NewContext)
            {
                context.Accounts.Add(toLaunchAccount);
                
                var launchInstance = new LaunchInstance(farmBuilder.Object, NewContext, instanceService.Object, dateTime.Object);
                await launchInstance.Execute();
            }
            
            var instances = await NewContext.Instances.Include(i => i.Account).ToListAsync();

            instances.Should().ContainSingle();

            var instance = instances[0];
            instance.Should().NotBeNull();
            instance.Should().BeOfType<Bot>();
            instance.Account.Should().BeEquivalentTo(toLaunchAccount);
            instance.StartTime.Should().Be(now);
            instance.LastUpdate.Should().Be(now);
        }

        [Fact]
        public async void Execute_LaunchSucceeds_MuleFirst()
        {
            var toLaunchBotAccount = BogusData.ValidBotAccounts.Generate();
            var toLaunchMuleAccount = BogusData.ValidMuleAccounts.Generate();

            var farmBuilder = new Mock<IFarmBuilder>();
            farmBuilder.Setup(f => f.BuildFarmSetup()).ReturnsAsync(
                new FarmSetup(new List<Account> {toLaunchBotAccount}, new List<Account> {toLaunchMuleAccount}));

            var instanceService = new Mock<IInstanceService>();
            instanceService.Setup(i => i.LaunchInstance(It.IsAny<Instance>())).ReturnsAsync(true);

            var now = DateTime.UtcNow;
            var dateTime = new Mock<IDateTime>();
            dateTime.Setup(d => d.UtcNow).Returns(now);

            using (var context = NewContext)
            {
                context.Accounts.Add(toLaunchBotAccount);
                context.Accounts.Add(toLaunchMuleAccount);
                    
                var launchInstance = new LaunchInstance(farmBuilder.Object, context, instanceService.Object, dateTime.Object);

                await launchInstance.Execute();
            }

            var instances = await NewContext
                .Instances
                .Include(i => i.Account)
                .ToListAsync();

            instances.Should().ContainSingle();

            var instance = instances[0];
            instance.Should().NotBeNull();
            instance.Should().BeOfType<Mule>();
            instance.Account.Should().BeEquivalentTo(toLaunchMuleAccount);
            instance.StartTime.Should().Be(now);
            instance.LastUpdate.Should().Be(now);
        }
    }
}