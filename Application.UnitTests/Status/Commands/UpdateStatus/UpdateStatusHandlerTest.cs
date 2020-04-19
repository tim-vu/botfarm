using System.Threading;
using Application.UnitTests.Common;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Status.UpdateStatus;
using Moq;
using Xunit;

namespace Application.UnitTests.Status.Commands.UpdateStatus
{
    public class UpdateStatusHandlerTest : TestBase
    {
        private static readonly Mock<IStateService> StateManager = new Mock<IStateService>();
        
        [Fact]
        public async void Handle_UpdateStatus_Start()
        {
            var updateStatusHandler = new UpdateStatusHandler(StateManager.Object);

            await updateStatusHandler.Handle(new FORFarm.Application.Status.UpdateStatus.UpdateStatus() {Running = true},
                CancellationToken.None);
            
            StateManager.Verify(s => s.Start());
        }

        [Fact]
        public async void Handle_UpdateStatus_Stop()
        {
            var updateStatusHandler = new UpdateStatusHandler(StateManager.Object);

            await updateStatusHandler.Handle(
                new FORFarm.Application.Status.UpdateStatus.UpdateStatus() {Running = false}, CancellationToken.None);
            
            StateManager.Verify(s => s.Stop());
        }
    }
}