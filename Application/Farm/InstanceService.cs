using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;
using FORFarm.Application.Proxies.Queries;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Farm
{
    public class InstanceService : IInstanceService
    {
        public static readonly TimeSpan HeartbeatTimeout = TimeSpan.FromSeconds(30);
        public static readonly TimeSpan LaunchTimeout = TimeSpan.FromMinutes(2);

        private readonly IDateTime _dateTime;
        private readonly IClientHandler _clientHandler;
        private readonly IFarmContext _context;
        private readonly IStateService _stateService;
        private readonly IMapper _mapper;
        
        public InstanceService(IDateTime dateTime, IClientHandler clientHandler, IStateService stateService, IMapper mapper, IFarmContext context)
        {
            _dateTime = dateTime;
            _clientHandler = clientHandler;
            _stateService = stateService;
            _mapper = mapper;
            _context = context;
        }

        public Expression<Func<Instance, bool>> IsConnected()
        {
            var startTimeout = _dateTime.UtcNow.Subtract(LaunchTimeout);
            var heartbeatTimeout = _dateTime.UtcNow.Subtract(HeartbeatTimeout);  

            return i => i.StartTime > startTimeout || i.LastUpdate >= heartbeatTimeout;
        }

        public async Task<bool> LaunchInstance(Instance instance)
        {
            var settingsTask = _context.ActiveFarmSettings;
            var stateTask = _stateService.GetFarmState();

            await Task.WhenAll(settingsTask, stateTask);

            var settings = settingsTask.Result;
            var state = stateTask.Result;

            string scriptName;
            if (instance is Bot)
            {
                scriptName = settings.BotScriptName;
            }
            else
            {
                scriptName = settings.MuleScriptName;
            }

            var args = new ClientStartArgs(instance.Account.Username, instance.Account.Password, scriptName)
            {
                Proxy = _mapper.Map<ProxyVm>(instance.Account.Proxy),
            };

            return await _clientHandler.StartClient(state.SocketAddress, args, settings.ApiKey);
        }
    }
}