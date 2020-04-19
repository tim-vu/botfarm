using System;
using System.Linq;
using System.Threading.Tasks;
using FORFarm.Application.Common;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Farm.FarmManager.FarmActions
{
    public class RelaunchOfflineInstance : IFarmAction
    {
        private readonly IFarmContext _context;
        private readonly IFarmBuilder _farmBuilder;
        private readonly IDateTime _dateTime;
        private readonly IInstanceService _instanceService;

        public RelaunchOfflineInstance(IFarmContext context, IDateTime dateTime, IFarmBuilder farmBuilder, IInstanceService instanceService)
        {
            _context = context;
            _dateTime = dateTime;
            _farmBuilder = farmBuilder;
            _instanceService = instanceService;
        }

        public Task<bool> IsNeeded()
        {
            return GetOfflineInstance().ContinueWith(t => t.Result != null);
        }

        public async Task Execute()
        {
            var instance = await GetOfflineInstance();

            instance.Tag = Guid.Empty;
            instance.StartTime = _dateTime.UtcNow;
            instance.LastUpdate = _dateTime.UtcNow;

            var result = await _instanceService.LaunchInstance(instance);

            if (!result)
                return;

            await _context.SaveChangesAsync();
        }

        private async Task<Instance> GetOfflineInstance()
        {
            var farm = await _farmBuilder.BuildFarmSetup();
            var accountIds = farm.Accounts.Select(a => a.ID).ToList();

            return await _context
                .Instances
                .Include(i => i.Account)
                .Where(i => accountIds.Contains(i.Account.ID))
                .Where(_instanceService.IsConnected().Not())
                .FirstOrDefaultAsync();
        }
    }
}