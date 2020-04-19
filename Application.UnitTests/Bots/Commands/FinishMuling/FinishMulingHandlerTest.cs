using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Bots.Commands.FinishMuling;
using FORFarm.Application.Common.Exceptions;
using FORFarm.Domain.Entities;
using Xunit;

namespace Application.UnitTests.Bots.Commands.FinishMuling
{
    public class FinishMulingHandlerTest : TestBase
    {
        [Fact]
        public void Handle_FinishMuling_NonExistingMuleRequest()
        {
            using (var context = NewContext)
            {
                var request = new FORFarm.Application.Bots.Commands.FinishMuling.FinishMuling()
                {
                    Tag = Guid.NewGuid()
                };
                
                var finishMulingHandler = new FinishMulingHandler(context);

                Func<Task> act = () => finishMulingHandler.Handle(request, CancellationToken.None);

                act.Should().Throw<NotFoundException>();
            }
        }

        [Fact]
        public async void Handle_FinishMuling_ExistingTag()
        {
            var bot = BogusData.Bots.Generate();
            var mule = BogusData.Mules.Generate();

            var muleRequest = new MuleRequest
            {
                Mule = mule,
                Bot = bot
            };

            await using (var context = NewContext)
            {
                context.MuleRequests.Add(muleRequest);
                await context.SaveChangesAsync();
            }

            const int goldTransferred = 500000;

            await using (var context = NewContext)
            {
                var request = new FORFarm.Application.Bots.Commands.FinishMuling.FinishMuling()
                {
                    Tag = bot.Tag,
                    GoldTransferred = goldTransferred
                };
                
                var finishMulingHandler = new FinishMulingHandler(context);

                await finishMulingHandler.Handle(request, CancellationToken.None);
            }

            await using (var context = NewContext)
            {
                context.MuleRequests.ToList().Should().BeEmpty();

                var uBot = context.Bots.Find(bot.ID);
                uBot.Should().NotBeNull();
                uBot.GoldEarned.Should().Be(bot.GoldEarned + goldTransferred);
            }
        }
    }
}