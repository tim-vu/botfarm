using System;
using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm.Bot;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FORFarm.Application.Bots.Commands.RegisterBot
{
    public class RegisterBotHandler : IRequestHandler<RegisterBot, BotResponse>
    {
        private readonly IFarmContext _context;
        private readonly ILogger<RegisterBotHandler> _logger;
        private readonly IDateTime _dateTime;
        private readonly IStateService _stateService;

        public RegisterBotHandler(IFarmContext context, ILogger<RegisterBotHandler> logger, IDateTime dateTime, IStateService stateService)
        {
            _context = context;
            _logger = logger;
            _dateTime = dateTime;
            _stateService = stateService;
        }

        public async Task<BotResponse> Handle(RegisterBot request, CancellationToken cancellationToken)
        {
            if (!await _stateService.IsRunning())
                return new BotResponse(Command.Terminate);

            var bot = await _context.Bots
                .Include(b => b.Account)
                .FirstOrDefaultAsync(b => b.Account.Username == request.Username, cancellationToken);

            if (bot == null)
            {
                return new BotResponse(Command.Terminate);
            }

            _logger.LogDebug($"Bot registered tag: {request.Tag}, username: {request.Username}");
            
            bot.Tag = request.Tag;
            bot.LastUpdate = _dateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new BotResponse(Command.Continue);
        }
    }
}