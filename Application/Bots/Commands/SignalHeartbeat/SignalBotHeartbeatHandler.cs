using System;
using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm.Bot;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FORFarm.Application.Bots.Commands.SignalHeartbeat
{
    public class SignalBotHeartbeatHandler : IRequestHandler<SignalBotHeartbeat, BotResponse>
    {
        private readonly IFarmContext _context;
        private readonly ILogger<SignalBotHeartbeatHandler> _logger;
        private readonly IDateTime _dateTime;
        private readonly IStateService _stateService;

        public SignalBotHeartbeatHandler(IFarmContext context, ILogger<SignalBotHeartbeatHandler> logger, IDateTime dateTime, IStateService stateService)
        {
            _context = context;
            _logger = logger;
            _dateTime = dateTime;
            _stateService = stateService;
        }

        public async Task<BotResponse> Handle(SignalBotHeartbeat request, CancellationToken cancellationToken)
        {
            if(!await _stateService.IsRunning())
                return new BotResponse(Command.Terminate);
            
            var bot = await _context.Bots.FirstOrDefaultAsync(b => b.Tag == request.Tag, cancellationToken);

            if (bot == null)
            {
                _logger.LogWarning($"Bot with tag {request.Tag} was not found");
                return new BotResponse(Command.Terminate);
            }

            bot.LastUpdate = _dateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return new BotResponse(Command.Continue);
        }
    }
}