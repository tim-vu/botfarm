using System.Linq;
using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Mules.Queries;
using FORFarm.Application.Mules.Queries.GetMules;
using Xunit;

namespace Application.UnitTests.Mules.Queries.GetMules
{
    public class GetMulesHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_GetMules()
        {
            var mules = BogusData.Mules.Generate(10);
            mules.ForEach(m => m.Account = BogusData.ValidMuleAccounts.Generate());

            using (var context = NewContext)
            {
                context.Mules.AddRange(mules);
                await context.SaveChangesAsync();
            }
            
            var getMulesHandler = new GetMulesHandler(NewContext, Mapper);

            var result = await getMulesHandler.Handle(new FORFarm.Application.Mules.Queries.GetMules.GetMules(),
                CancellationToken.None);

            result.Should().BeEquivalentTo(mules.Select(m => Mapper.Map<MuleVm>(m)).ToList());
        }
    }
}