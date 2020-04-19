using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Settings;
using FORFarm.Application.Settings.Queries.GetSettings;
using FORFarm.Domain.Entities;
using Moq;
using Xunit;

namespace Application.UnitTests.Settings.Queries.GetSettings
{
    public class GetSettingsHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_GetSettings()
        {
            var settings = Clone(Data.Settings);
            settings.ID = 1;

            using (var context = NewContext)
            {
                context.Update(settings);
                await context.SaveChangesAsync();
            }
            
            var getSettingsHandler = new GetSettingsHandler(Mapper, NewContext);

            var result = await getSettingsHandler.Handle(new FORFarm.Application.Settings.Queries.GetSettings.GetSettings(),
                CancellationToken.None);
            
            result.Should().BeEquivalentTo(Mapper.Map<SettingsVm>(settings));
        }
    }
}