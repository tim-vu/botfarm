using System.Linq;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FORFarm.Application.Farm.FarmManager.FarmActions
{
    public class LaunchInstance : IFarmAction
    {
        private readonly IFarmBuilder _farmBuilder;
        private readonly IInstanceService _instanceService;
        private readonly IFarmContext _context;
        private readonly IDateTime _dateTime;

        public LaunchInstance(IFarmBuilder farmBuilder, IFarmContext context, IInstanceService instanceService, IDateTime dateTime)
        {
            _farmBuilder = farmBuilder;
            _context = context;
            _instanceService = instanceService;
            _dateTime = dateTime;
        }

        public async Task<bool> IsNeeded()
        {
            var farm = await GetToLaunchFarm();

            return farm.Mules.Count > 0 || farm.Bots.Count > 0;
        }

        public async Task Execute()
        {
            var farm = await GetToLaunchFarm();

            var account = farm.Mules.Count > 0 ? farm.Mules.First() : farm.Bots.First();

            Instance instance;

            if (account.Mule)
                instance = new Mule();
            else
                instance = new Bot();

            instance.Account = account;
            instance.StartTime = _dateTime.UtcNow;
            instance.LastUpdate = _dateTime.UtcNow;

            var result = await _instanceService.LaunchInstance(instance);

            if (!result)
                return;

            _context.Instances.Add(instance);
            await _context.SaveChangesAsync();
        }

        private async Task<FarmSetup> GetToLaunchFarm()
        {
            var instances = await _context
                .Instances
                .Include(i => i.Account)
                .ThenInclude(a => a.Proxy)
                .ToListAsync();

            var farm = await _farmBuilder.BuildFarmSetup();
            return farm.RemoveAccounts(instances.Select(i => i.Account));
        }
    }
}