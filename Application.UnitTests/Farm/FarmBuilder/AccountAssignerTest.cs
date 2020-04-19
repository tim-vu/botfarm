using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Farm;
using FORFarm.Domain.Entities;
using Xunit;

namespace Application.UnitTests.Farm.FarmBuilder
{
    public class AssignAccountsTest : FarmBuilderTestBase
    {

        [Fact]
        public async void AssignAccounts_NotEnoughValidMules()
        {
            var nonMemberMules = BogusData.CreateFaker(true, false, false).Generate(MinActiveMules);
            var bannedMules = BogusData.CreateFaker(true, true, true).Generate(MinActiveMules);

            var bots = BogusData.CreateFaker(false, true, false).Generate(MaxActiveBots);
            
            using(var context = NewContext){
                context.Accounts.AddRange(nonMemberMules);
                context.Accounts.AddRange(bannedMules);
                context.Accounts.AddRange(bots);

                await context.SaveChangesAsync();
            }

            var farmSetup = await AccountAssigner.AssignAccounts(NewContext, Settings);

            farmSetup.Should().NotBeNull();
            farmSetup.IsEmpty().Should().BeTrue();
        }

        [Fact]
        public async void AssignAccounts_NotEnoughValidBots()
        {
            var mules = BogusData.CreateFaker(true, true, false).Generate(MaxActiveMules);

            var nonMemberMules = BogusData.CreateFaker(false, false, true).Generate(1);
            var bannedBots = BogusData.CreateFaker(false, true, true).Generate(1);

            using (var context = NewContext)
            {
                context.Accounts.AddRange(mules);
                context.Accounts.AddRange(nonMemberMules);
                context.Accounts.AddRange(bannedBots);

                await context.SaveChangesAsync();
            }

            var farmSetup = await AccountAssigner.AssignAccounts(NewContext, Settings);

            farmSetup.Should().NotBeNull();
            farmSetup.IsEmpty().Should().BeTrue();
        }

        [Fact]
        public async void AssignAccounts_ValidMuleExcess()
        {
            var mules = BogusData.ValidMuleAccounts.Generate(MaxActiveMules * 2);
            var bots = BogusData.ValidBotAccounts.Generate(MaxActiveBots);

            using (var context = NewContext)
            {
                context.Accounts.AddRange(mules);
                context.Accounts.AddRange(bots);

                await context.SaveChangesAsync();
            }

            var farmSetup = await AccountAssigner.AssignAccounts(NewContext, Settings);

            farmSetup.Should().NotBeNull();
            farmSetup.IsEmpty().Should().BeFalse();
            farmSetup.Mules.Should().HaveCount(MaxActiveMules);
        }

        [Fact]
        public async void AssignAccounts_ValidBotsExcess()
        {
            var mules = BogusData.ValidMuleAccounts.Generate(MaxActiveMules);
            var bots = BogusData.ValidBotAccounts.Generate(MaxActiveBots * 2);

            using (var context = NewContext)
            {
                context.Accounts.AddRange(mules);
                context.Accounts.AddRange(bots);

                await context.SaveChangesAsync();
            }

            var farmSetup = await AccountAssigner.AssignAccounts(NewContext, Settings);

            farmSetup.Should().NotBeNull();
            farmSetup.IsEmpty().Should().BeFalse();
            farmSetup.Bots.Should().HaveCount(MaxActiveBots);
        }
        
    }
}