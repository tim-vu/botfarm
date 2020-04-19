using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Farm.FarmManager.FarmActions;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FORFarm.Application.Farm.FarmManager
{
    public class FarmService : IFarmService
    {
        private static Random Random { get; } = new Random();

        private readonly IFarmContext _context;
        private readonly IServiceProvider _services;
        private readonly IStateService _stateService;

        public FarmService(
            IFarmContext context, 
            IServiceProvider services, IStateService stateService)
        {
            _context = context;
            _services = services;
            _stateService = stateService;
        }
        
        public async Task<Position> GetMulePosition()
        {
            return await _context.Positions.Skip(Random.Next(0, _context.Positions.Count())).FirstOrDefaultAsync();
        }

        private static readonly IReadOnlyList<Type> NonRunningFarmActions = new List<Type>
        {
            typeof(DeleteOfflineInstance),
            typeof(DoNothing)
        }; 
        
        private static readonly IReadOnlyList<Type> RunningFarmActions = new List<Type>
        {
            typeof(StopFarmWhenEmpty),
            typeof(DeleteOfflineNonFarmInstance),
            typeof(RelaunchOfflineInstance),
            typeof(LaunchInstance),
            typeof(DoNothing)
        };
        
        public async Task<IFarmAction> GetFarmAction()
        {
            var running = await _stateService.IsRunning();

            if (!running)
            {
                foreach (var type in NonRunningFarmActions)
                {
                    var action = (IFarmAction)ActivatorUtilities.CreateInstance(_services, type);

                    if (await action.IsNeeded())
                        return action;
                }

                return null;
            }
            
            foreach (var type in RunningFarmActions)
            {
                var action = (IFarmAction)ActivatorUtilities.CreateInstance(_services, type);

                if (await action.IsNeeded())
                    return action;
            }

            return null;
        }

        private class DoNothing : IFarmAction
        {
            public Task<bool> IsNeeded()
            {
                return Task.FromResult(true);
            }

            public Task Execute()
            {
                return Task.Delay(5000);
            }
        }
        
        //TODO: move this
        public static void DeleteInstance(IFarmContext context, Instance instance)
        {
            switch (instance)
            {
                case Bot bot:
                    bot.Account.GoldEarned += bot.GoldEarned;
                    bot.Account.Runtime += bot.LastUpdate.Subtract(bot.StartTime);
                    break;
            }

            context.Instances.Remove(instance);
        }
    }
}