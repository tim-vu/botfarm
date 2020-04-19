using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using MediatR;

namespace FORFarm.Application.Status.UpdateStatus
{
    public class UpdateStatus : IRequest
    {
        public bool Running { get; set; }
    }
    
    public class UpdateStatusHandler : IRequestHandler<UpdateStatus>
    {
        private readonly IStateService _stateService;

        public UpdateStatusHandler(IStateService stateService)
        {
            _stateService = stateService;
        }

        public Task<Unit> Handle(UpdateStatus request, CancellationToken cancellationToken)
        {
            if (request.Running)
            {
                _stateService.Start();
            }
            else
            {
                _stateService.Stop();
            }

            return Task.FromResult(Unit.Value);
        }
    }
}