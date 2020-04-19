using System;
using System.Linq;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Farm
{
    public class StateService : IStateService
    {
        private const int StateId = 1;
        
        private readonly IFarmContext _context;
        private readonly IDateTime _dateTime;
        private readonly ISettingsService _settingsService;
        private readonly IFarmBuilder _farmBuilder;
        private readonly IClientHandler _clientHandler;

        public StateService(IFarmContext context, IDateTime dateTime, ISettingsService settingsService, IFarmBuilder farmBuilder, IClientHandler clientHandler)
        {
            _context = context;
            _dateTime = dateTime;
            _settingsService = settingsService;
            _farmBuilder = farmBuilder;
            _clientHandler = clientHandler;
        }

        public Task<FarmState> GetFarmState()
        {
            return _context.FarmStates.FindAsync(StateId).AsTask();
        }

        public Task<bool> IsRunning()
        {
            return GetFarmState().ContinueWith(t => t.Result.Running);
        }
            
        public async Task<bool> CanStart()
        {
            if (await IsRunning())
                return false;

            if (!await _settingsService.AreSettingsReady())
                return false;

            return !(await _farmBuilder.BuildFarmSetup()).IsEmpty();
        }

        public async Task Start()
        {
            if (!await CanStart())
                return;

            var settings = await _context.FarmSettings;

            var launchers = await _clientHandler.GetLaunchers(settings.ApiKey);
            var launcher = launchers?.FirstOrDefault(l => l.Hostname == settings.Hostname);

            if (launcher == null)
                return;

            _settingsService.ActivateFarmSettings();

            var state = await GetFarmState();
            state.Running = true;
            state.Start = _dateTime.UtcNow;
            state.SocketAddress = launcher.SocketAddress;

            await _context.SaveChangesAsync();
        }
        
        public Task Stop()
        {
            return _context.FarmStates.Where(s => s.ID == StateId).UpdateFromQueryAsync(s => new FarmState {Running = false}).ContinueWith(r => Task.CompletedTask);
        }
    }
}