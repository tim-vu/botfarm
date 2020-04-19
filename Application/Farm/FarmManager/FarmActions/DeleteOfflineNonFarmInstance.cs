using System.Linq;
using System.Threading.Tasks;
using FORFarm.Application.Common;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Farm.FarmManager.FarmActions
{
    public class DeleteOfflineNonFarmInstance : IFarmAction
    {
        private readonly IFarmContext _context;
        private readonly IStateService _stateService;
        private readonly IInstanceService _instanceService;
        private readonly IFarmBuilder _farmBuilder;
        
        public DeleteOfflineNonFarmInstance(IFarmContext context, IInstanceService instanceService, IFarmBuilder farmBuilder, IStateService stateService)
        {
            _context = context;
            _instanceService = instanceService;
            _farmBuilder = farmBuilder;
            _stateService = stateService;
        }

        public Task<bool> IsNeeded()
        {
            return GetOfflineNonFarmInstance().ContinueWith(t => t.Result != null);
        }

        public async Task Execute()
        {
            var instance = await GetOfflineNonFarmInstance();
            
            //TODO: refactor
            FarmService.DeleteInstance(_context, instance);

            await _context.SaveChangesAsync();
        }

        private async Task<Instance> GetOfflineNonFarmInstance()
        {
            var farm = await _farmBuilder.BuildFarmSetup();
            var accountIds = farm.Accounts.Select(i => i.ID).ToList();

            return await _context
                .Instances
                .Include(i => i.Account)
                .Where(_instanceService.IsConnected().Not())
                .Where(i => !accountIds.Contains(i.ID))
                .FirstOrDefaultAsync();
        }
    }
}