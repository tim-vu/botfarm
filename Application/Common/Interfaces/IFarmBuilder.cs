using System.Threading.Tasks;
using FORFarm.Application.Common.Models.Farm;

namespace FORFarm.Application.Common.Interfaces
{
    public interface IFarmBuilder
    {
        Task<FarmSetup> BuildFarmSetup();
    }
}