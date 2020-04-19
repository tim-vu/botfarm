using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORFarm.Application.Common;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FORFarm.Application.Farm.FarmManager.FarmActions
{
    public class DeleteOfflineInstance : IFarmAction
    {
        private readonly IFarmContext _context;
        private readonly IStateService _stateService;
        private readonly ILogger<DeleteOfflineInstance> _logger;
        private readonly IInstanceService _instanceService;

        public DeleteOfflineInstance(IFarmContext context, IStateService stateService, ILogger<DeleteOfflineInstance> logger, IInstanceService instanceService)
        {
            _context = context;
            _stateService = stateService;
            _logger = logger;
            _instanceService = instanceService;
        }
        
        public async Task<bool> IsNeeded()
        {
            if (await _stateService.IsRunning())
                return false;

            var instances = await GetOfflineInstances();
            return instances.Count > 0;
        }

        public async Task Execute()
        {
            var offlineInstances = await GetOfflineInstances();
            
            //TODO: refactor
            offlineInstances.ForEach(i => FarmService.DeleteInstance(_context, i));

            if (offlineInstances.Count > 0)
            {
                _logger.LogDebug($"Deleting {offlineInstances.Count} instances which aren't connected");

                await _context.SaveChangesAsync();
            }
        }

        private Task<List<Instance>> GetOfflineInstances()
        {
            return _context.Instances
                .Include(i => i.Account)
                .Where(_instanceService.IsConnected().Not())
                .ToListAsync();
        }
    }
}