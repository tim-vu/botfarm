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

namespace FORFarm.Application.Mules.Commands.RegisterMule
{
    public class RegisterMuleHandler : IRequestHandler<RegisterMule, MuleResponse>
    {
        private readonly IFarmService _farmService;
        private readonly IFarmContext _context;
        private readonly ILogger<RegisterMuleHandler> _logger;
        private readonly IDateTime _dateTime;
        private readonly IStateService _stateService;

        public RegisterMuleHandler(IFarmService farmService, IFarmContext context, ILogger<RegisterMuleHandler> logger, IDateTime dateTime, IStateService stateService)
        {
            _farmService = farmService;
            _context = context;
            _logger = logger;
            _dateTime = dateTime;
            _stateService = stateService;
        }

        public async Task<MuleResponse> Handle(RegisterMule request, CancellationToken cancellationToken)
        {
            if (!await _stateService.IsRunning())
                return new MuleResponse(Command.Terminate);

            var mule = await _context.Mules
                .Include(m => m.Account)
                .FirstOrDefaultAsync(m => m.Account.Username == request.Username, cancellationToken);
            
            if (mule == null)
            {
                return new MuleResponse(Command.Terminate);
            }

            var position = await _farmService.GetMulePosition();
            
            _logger.LogDebug($"Mule registered tag: {request.Tag}, username: {request.Username}");
            
            mule.Tag = request.Tag;
            mule.Position = position;
            mule.DisplayName = request.DisplayName;
            mule.LastUpdate = _dateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            
            return new MuleResponse(Command.Continue, position);
        }
    }
}