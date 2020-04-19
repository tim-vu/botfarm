using System;
using System.Collections.Generic;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Domain.Entities;
using Xunit;

namespace Application.UnitTests.Farm.SettingsManager
{
    public class SettingsManagerTest : TestBase
    {
        
        private static readonly FarmSettings Settings2 = new FarmSettings
        {
            BotScriptName = "ForFarm",
            MuleScriptName = "ForFarm_Mule",
            ApiKey = "1O7F660L7FXBB0X8KS6BXB5BOH3XWHWIRFQ7Q3ASQRPF19F2E9CYE020T51YDDYZTOT6KZ",
            Hostname = "MyHostname"
        };

        public static IEnumerable<object[]> GetNotReadyFarmSettings()
        {
            var settings = Clone(Settings2);
            settings.BotScriptName = null;
            yield return new object[]{settings};

            settings = Clone(Settings2);
            settings.MuleScriptName = null;
            yield return new object[] {settings};

            settings = Clone(Settings2);
            settings.ApiKey = null;
            yield return new object[] {settings};

            settings = Clone(Settings2);
            settings.Hostname = null;
            yield return new object[] {settings};
        }
        
        [Theory]
        [MemberData(nameof(GetNotReadyFarmSettings))]
        public async void AreSettingsReady_NotReady(FarmSettings farmSettings)
        {
            using (var context = NewContext)
            {
                farmSettings.ID = 1;
                context.Update(farmSettings);
                await context.SaveChangesAsync();
            }
            
            var settingsManager = new FORFarm.Application.Farm.SettingsService(NewContext);
            var result = await settingsManager.AreSettingsReady();

            result.Should().BeFalse();
        }

        [Fact]
        public async void GetActiveFarmSettings()
        {
            var settings = Clone(Data.Settings);
            
            using (var context = NewContext)
            {
                settings.ID = 2;
                context.Update(settings);
                await context.SaveChangesAsync();
            }

            var settingsManager = new FORFarm.Application.Farm.SettingsService(NewContext);
            var activeSettings = await settingsManager.GetActiveFarmSettings();

            activeSettings.Should().NotBeNull();
            activeSettings.Should().BeEquivalentTo(settings);
        }

        [Fact]
        public async void ActivateFarmSettings()
        {
            var settings = Clone(Data.Settings);
            
            using (var context = NewContext)
            {
                settings.ID = 1;
                context.Update(settings);
                await context.SaveChangesAsync();
            }

            using (var context = NewContext)
            {
                var settingsManager = new FORFarm.Application.Farm.SettingsService(context);
                settingsManager.ActivateFarmSettings();
                await context.SaveChangesAsync();
            }

            var activeSettings = await NewContext.FarmSettingsTable.FindAsync(2);
            
            activeSettings.Should().BeEquivalentTo(settings, o => o.Excluding(s => s.ID));
        }
    }
}