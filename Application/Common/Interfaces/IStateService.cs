
using System.Threading.Tasks;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Common.Interfaces
{
    public interface IStateService
    {
        Task<FarmState> GetFarmState();
        
        Task<bool> IsRunning();

        Task<bool> CanStart();
        
        Task Start();
        
        Task Stop();
    }
}