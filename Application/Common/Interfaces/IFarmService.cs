using System.Threading.Tasks;
using FORFarm.Application.Farm.FarmManager.FarmActions;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Common.Interfaces
{
    public interface IFarmService
    {
        Task<Position> GetMulePosition();

        Task<IFarmAction> GetFarmAction();
    }
}