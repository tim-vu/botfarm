using System.Threading.Tasks;

namespace FORFarm.Application.Farm.FarmManager.FarmActions
{
    public interface IFarmAction
    {
        Task<bool> IsNeeded()
        {
            return Task.FromResult(false);
        }

        Task Execute();
    }
}