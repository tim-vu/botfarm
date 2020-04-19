using System.Threading;
using Application.UnitTests.Common;
using FluentAssertions;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Status;
using FORFarm.Application.Status.GetStatus;
using Moq;
using Xunit;

namespace Application.UnitTests.Status.Queries
{
    public class GetStatusHandlerTest : TestBase
    {
        [Fact]
        public async void Handle_GetStatus()
        {
            var status = new StatusVm
            {
                Running = false,
                CanStart = true
            };
            
            var stateManager = new Mock<IStateService>();
            stateManager.Setup(s => s.IsRunning()).ReturnsAsync(status.Running);
            stateManager.Setup(s => s.CanStart()).ReturnsAsync(status.CanStart);
            
            var getStatusHandler = new GetStatusHandler(stateManager.Object);

            var result = await getStatusHandler.Handle(new GetStatus(), CancellationToken.None);
            
            result.Should().BeEquivalentTo(status);
        }
    }
}