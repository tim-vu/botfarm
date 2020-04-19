using System;
using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Bots.Commands.RequestMuling;
using FORFarm.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.UnitTests.Bots.Commands.RequestMuling
{
    public class RequestMulingHandlerTest : TestBase
    {
        private static readonly Random Random = new Random();
        
        private static readonly Mock<ILogger<RequestMulingHandler>> _logger = new Mock<ILogger<RequestMulingHandler>>();

        [Fact]
        public async void Handle_RequestMuling_NotRunning()
        {
            var stateService = new Mock<IStateService>();
            stateService.Setup(s => s.IsRunning()).ReturnsAsync(false);


            var instanceService = new Mock<IInstanceService>();

            var random = new Mock<IRandom>();

            
            RequestMulingResponse response;
            using (var context = NewContext)
            {
                var request = new FORFarm.Application.Bots.Commands.RequestMuling.RequestMuling();
                
                var requestMulingHandler = new RequestMulingHandler(stateService.Object, context, _logger.Object, instanceService.Object, random.Object);

                response = await requestMulingHandler.Handle(request, CancellationToken.None);
            }

            response.Should().NotBeNull();
            response.Accepted.Should().BeFalse();
        }

        [Fact]
        public async void Handle_RequestMuling_Running_NonExistingTag()
        {
            var stateService = new Mock<IStateService>();
            stateService.Setup(s => s.IsRunning()).ReturnsAsync(true);

            var instanceService = new Mock<IInstanceService>();
            var random = new Mock<IRandom>();

            RequestMulingResponse response;
            using (var context = NewContext)
            {
                var request = new FORFarm.Application.Bots.Commands.RequestMuling.RequestMuling()
                {
                    Tag = Guid.NewGuid()
                };
                
                var requestMulingHandler = new RequestMulingHandler(stateService.Object, context, _logger.Object, instanceService.Object, random.Object);

                response = await requestMulingHandler.Handle(request, CancellationToken.None);
            }

            response.Should().NotBeNull();
            response.Accepted.Should().BeFalse();
        }

        [Fact]
        public async void Handle_RequestMuling_Running_ExistingTag_NoMulesConnected()
        {
            var stateService = new Mock<IStateService>();
            stateService.Setup(s => s.IsRunning()).ReturnsAsync(true);

            var instanceService = new Mock<IInstanceService>();
            instanceService.Setup(i => i.IsConnected()).Returns(i => false);
            
            var random = new Mock<IRandom>();

            var bot = BogusData.Bots.Generate();
            var mule = BogusData.Mules.Generate();
            
            using (var context = NewContext)
            {
                context.Bots.Add(bot);
                context.Mules.Add(mule);
                await context.SaveChangesAsync();
            }
            
            RequestMulingResponse response;
            using (var context = NewContext)
            {
                var request = new FORFarm.Application.Bots.Commands.RequestMuling.RequestMuling()
                {
                    Tag = bot.Tag
                };
                
                var requestMulingHandler = new RequestMulingHandler(stateService.Object, context, _logger.Object, instanceService.Object, random.Object);

                response = await requestMulingHandler.Handle(request, CancellationToken.None);
            }

            response.Should().NotBeNull();
            response.Accepted.Should().BeFalse();
        }

        [Fact]
        public async void Handle_RequestMuling_Running_ExistingTag_MulesConnected()
        {
            var stateService = new Mock<IStateService>();
            stateService.Setup(s => s.IsRunning()).ReturnsAsync(true);

            var instanceService = new Mock<IInstanceService>();
            instanceService.Setup(i => i.IsConnected()).Returns(i => true);

            var bot = BogusData.Bots.Generate();
            var mules = BogusData.Mules.Generate(3);

            var muleIndex = Random.Next(mules.Count);
            var random = new Mock<IRandom>();
            random.Setup(r => r.Next(It.Is<int>(v => v == 0), It.Is<int>(v => v == mules.Count))).Returns(muleIndex);

            using (var context = NewContext)
            {
                context.Bots.Add(bot);
                context.Mules.AddRange(mules);
                await context.SaveChangesAsync();
            }
            
            RequestMulingResponse response;
            using (var context = NewContext)
            {
                var request = new FORFarm.Application.Bots.Commands.RequestMuling.RequestMuling()
                {
                    Tag = bot.Tag
                };
                
                var requestMulingHandler = new RequestMulingHandler(stateService.Object, context, _logger.Object, instanceService.Object, random.Object);

                response = await requestMulingHandler.Handle(request, CancellationToken.None);
            }

            var selectedMule = mules[muleIndex];
            
            response.Should().NotBeNull();
            response.Accepted.Should().BeTrue();
            response.MuleInfo.Should().NotBeNull();
            response.MuleInfo.DisplayName.Should().Be(selectedMule.DisplayName);
            response.MuleInfo.Position.Should().BeEquivalentTo(selectedMule.Position);
            response.MuleInfo.World.Should().Be(selectedMule.World);
        }
    }
}