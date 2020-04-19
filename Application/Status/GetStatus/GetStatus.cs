using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using MediatR;

namespace FORFarm.Application.Status.GetStatus
{
    public class GetStatus : IRequest<StatusVm>
    {
            
    }
    
    public class GetStatusHandler : IRequestHandler<GetStatus, StatusVm>
    {
        private readonly IStateService _stateService;
        
        public GetStatusHandler(IStateService stateService)
        {
            _stateService = stateService;
        }

        public async Task<StatusVm> Handle(GetStatus request, CancellationToken cancellationToken)
        {
            var runningTask = _stateService.IsRunning();
            var canStartTask = _stateService.CanStart();

            await Task.WhenAll(runningTask, canStartTask);
            
            return new StatusVm
            {
                Running = runningTask.Result,
                CanStart = canStartTask.Result
            };
        }
    }
}