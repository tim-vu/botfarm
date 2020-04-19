using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;

namespace FORFarm.Application.Farm
{
    public class FarmBuilder : IFarmBuilder
    {
        private readonly IFarmContext _context;
        private readonly ISettingsService _settingsService;

        private FarmSetup _farmSetup;
        
        public FarmBuilder(IFarmContext context, ISettingsService settingsServiceService)
        {
            _context = context;
            _settingsService = settingsServiceService;
        }

        public async Task<FarmSetup> BuildFarmSetup()
        {
            if (_farmSetup != null)
                return _farmSetup;

            var settings = await _context.FarmSettings;
            var candidateFarm = await AccountAssigner.AssignAccounts(_context, settings);

            if (candidateFarm.IsEmpty())
            {
                return _farmSetup = candidateFarm;
            }

            if (await ProxyAssigner.AssignProxies(_context, candidateFarm, settings))
                return _farmSetup = candidateFarm;

            return _farmSetup = FarmSetup.EmptyFarm;
        }
    }
}
