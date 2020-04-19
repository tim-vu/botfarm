using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace FORFarm.Application.Farm.FarmManager.FarmActions
{
    public class StopFarmWhenEmpty : IFarmAction
    {
        private readonly IFarmBuilder _farmBuilder;
        private readonly IStateService _stateService;
        private readonly ILogger<StopFarmWhenEmpty> _logger;

        public StopFarmWhenEmpty(IFarmBuilder farmBuilder, IStateService stateService, ILogger<StopFarmWhenEmpty> logger)
        {
            _farmBuilder = farmBuilder;
            _stateService = stateService;
            _logger = logger;
        }

        public Task<bool> IsNeeded()
        {
            return _farmBuilder.BuildFarmSetup().ContinueWith(t => t.Result.IsEmpty());
        }

        public Task Execute()
        {
            _logger.LogDebug("Stopping the farm as the farm is empty");
            
            return _stateService.Stop();
        }
    }
}