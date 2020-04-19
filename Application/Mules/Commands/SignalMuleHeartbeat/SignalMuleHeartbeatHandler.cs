using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm.Bot;
using FORFarm.Application.Common.Models.Farm.Mule;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FORFarm.Application.Mules.Commands.SignalMuleHeartbeat
{
    public class SignalMuleHeartbeatHandler : IRequestHandler<SignalMuleHeartbeat, MuleHeartbeatResponse>
    {
        private readonly IFarmContext _context;
        private readonly ILogger<SignalMuleHeartbeatHandler> _logger;
        private readonly IDateTime _dateTime;
        private readonly IStateService _stateService;
        
        public SignalMuleHeartbeatHandler(IFarmContext context, ILogger<SignalMuleHeartbeatHandler> logger, IDateTime dateTime, IStateService stateService)
        {
            _context = context;
            _logger = logger;
            _dateTime = dateTime;
            _stateService = stateService;
        }

        public async Task<MuleHeartbeatResponse> Handle(SignalMuleHeartbeat request, CancellationToken cancellationToken)
        {
            if(!await _stateService.IsRunning())
                return new MuleHeartbeatResponse(Command.Terminate);

            var mule = await _context.Mules.FirstOrDefaultAsync(m => m.Tag == request.Tag, cancellationToken);

            if (mule == null)
            {
                _logger.LogDebug($"Mule with tag: {request.Tag} was not found");
                return new MuleHeartbeatResponse(Command.Terminate);
            }

            var muleRequests = await _context.MuleRequests
                .Where(r => r.MuleId == mule.ID)
                .Select(r => r.Bot.DisplayName)
                .ToListAsync(cancellationToken);
            
            mule.LastUpdate = _dateTime.UtcNow;
            mule.Gold = request.Gold;

            await _context.SaveChangesAsync(cancellationToken);

            return new MuleHeartbeatResponse(Command.Continue, muleRequests);
        }
    }
}