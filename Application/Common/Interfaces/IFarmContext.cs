using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Accounts.Queries.GetAccounts;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FORFarm.Application.Common.Interfaces
{
    public interface IFarmContext
    {
         DbSet<Account> Accounts { get; }
         
         DbSet<Proxy> Proxies { get; }
         
         DbSet<FarmState> FarmStates { get; }
         
         DbSet<Position> Positions { get; }
         
         DbSet<Bot> Bots { get; }
         
         DbSet<Mule> Mules { get; }
         
         DbSet<Instance> Instances { get; }
         
         DbSet<MuleRequest> MuleRequests { get; }
         
         Task<FarmSettings> FarmSettings { get; }
         
         Task<FarmSettings> ActiveFarmSettings { get; }
         
         DbSet<FarmSettings> FarmSettingsTable { get; }

         EntityEntry<T> Entry<T>(T entity) where T : class;

         Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}