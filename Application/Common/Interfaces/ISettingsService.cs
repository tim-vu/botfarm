using System.Threading.Tasks;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Common.Interfaces
{
    public interface ISettingsService
    {
        Task<bool> AreSettingsReady();

        void ActivateFarmSettings();
    }
}