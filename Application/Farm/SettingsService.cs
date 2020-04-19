using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace FORFarm.Application.Farm
{
    public class SettingsService : ISettingsService
    {
        private readonly IFarmContext _context;

        public SettingsService(IFarmContext context)
        {
            _context = context;
        }

        public Task<FarmSettings> GetFarmSettings()
        {
            return _context.FarmSettingsTable.FindAsync(1).AsTask();
        }

        public async Task<bool> AreSettingsReady()
        {
            var settings = await _context.FarmSettings;

            return !string.IsNullOrEmpty(settings.BotScriptName) &&
                   !string.IsNullOrEmpty(settings.MuleScriptName) &&
                   !string.IsNullOrEmpty(settings.ApiKey) &&
                   !string.IsNullOrEmpty(settings.Hostname);
        }

        public Task<FarmSettings> GetActiveFarmSettings()
        {
            return _context.FarmSettingsTable.FindAsync(2).AsTask();
        }

        public async void ActivateFarmSettings()
        {
            var settings = await _context.FarmSettingsTable.AsNoTracking().FirstAsync();
            settings.ID = 2;
            _context.FarmSettingsTable.Update(settings);
        }
        
    }
}