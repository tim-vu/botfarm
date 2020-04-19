using System.Threading;
using Application.UnitTests.Common;
using AutoMapper;
using FluentAssertions;
using FORFarm.Application.Settings;
using FORFarm.Application.Settings.Commands.UpdateSettings;
using FORFarm.Domain.Entities;
using Xunit;

namespace Application.UnitTests.Settings.Commands.UpdateSettings
{
    public class UpdateSettingsHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_UpdateSettings()
        {
            var settings = Clone(Data.Settings);
            var settingsVm = Mapper.Map<SettingsVm>(settings);

            var updateSettingsHandler = new UpdateSettingsHandler(NewContext, Mapper);
            
            await updateSettingsHandler.Handle(
                new FORFarm.Application.Settings.Commands.UpdateSettings.UpdateSettings(settingsVm),
                CancellationToken.None);


            var uSettings = await NewContext.FarmSettingsTable.FindAsync(1);
            Mapper.Map<SettingsVm>(uSettings).Should().BeEquivalentTo(settingsVm);
        }
    }
}