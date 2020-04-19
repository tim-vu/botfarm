using System.Linq;
using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Bots.Queries;
using FORFarm.Application.Bots.Queries.GetBots;
using Xunit;

namespace Application.UnitTests.Bots.Queries.GetBots
{
    public class GetBotsHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_GetBots()
        {
            var bots = BogusData.Bots.Generate(10);
            bots.ForEach(b => b.Account = BogusData.ValidBotAccounts.Generate());

            using (var context = NewContext)
            {
                context.Bots.AddRange(bots);
                await context.SaveChangesAsync();
            }
            
            var getBotsHandler = new GetBotHandler(NewContext, Mapper);

            var result = await getBotsHandler.Handle(new FORFarm.Application.Bots.Queries.GetBots.GetBots(),
                CancellationToken.None);

            result.Should().BeEquivalentTo(bots.Select(b => Mapper.Map<BotVm>(b)).ToList());
        }
    }
}