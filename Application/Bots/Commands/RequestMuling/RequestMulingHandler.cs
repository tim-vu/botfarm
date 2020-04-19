using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FORFarm.Application.Bots.Commands.RequestMuling
{
    public class RequestMulingHandler : IRequestHandler<RequestMuling, RequestMulingResponse>
    {
        private readonly IFarmContext _context;
        private readonly IStateService _stateService;
        private readonly ILogger<RequestMulingHandler> _logger;
        private readonly IInstanceService _instanceService;
        private readonly IRandom _random;

        public RequestMulingHandler(IStateService stateService, IFarmContext context, ILogger<RequestMulingHandler> logger, IInstanceService instanceService, IRandom random)
        {
            _stateService = stateService;
            _context = context;
            _logger = logger;
            _instanceService = instanceService;
            _random = random;
        }

        public async Task<RequestMulingResponse> Handle(RequestMuling request, CancellationToken cancellationToken)
        {
            if(!await _stateService.IsRunning())
                return new RequestMulingResponse();

            var bot = await _context.Bots.FirstOrDefaultAsync(b => b.Tag == request.Tag, cancellationToken);

            if (bot == null)
            {
                _logger.LogWarning($"Bot with tag {request.Tag} was not found");
                return new RequestMulingResponse();
            }

            var mules = await _context.Mules
                .Include(m => m.Position)
                .Where(_instanceService.IsConnected())
                .Cast<Mule>()
                .ToListAsync(cancellationToken);

            if (mules.Count == 0)
            {
                return new RequestMulingResponse();
            }

            var selectedMule = mules[_random.Next(0, mules.Count)];
            
            var muleRequest = new MuleRequest
            {
                Mule = selectedMule,
                Bot = bot
            };

            _context.MuleRequests.Add(muleRequest);

            await _context.SaveChangesAsync(cancellationToken);
            
            return new RequestMulingResponse(new MuleInfo(selectedMule.DisplayName, selectedMule.Position, selectedMule.World));
        }
    }
}