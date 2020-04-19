using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Persistence
{
    public class ForFarmDbContext : DbContext, IFarmContext
    {
        public ForFarmDbContext(DbContextOptions<ForFarmDbContext> options) : base(options)
        {

        }
        
        public DbSet<Account> Accounts { get; set;  }

        public DbSet<Proxy> Proxies { get; set; }

        public DbSet<FarmState> FarmStates { get; set; }

        public DbSet<Bot> Bots { get; set; }
        
        public DbSet<Mule> Mules { get; set; }
        
        public DbSet<Instance> Instances { get; set; }
        
        public DbSet<Position> Positions { get; set; }

        public DbSet<MuleRequest> MuleRequests { get; set; }
        
        public Task<FarmSettings> FarmSettings => FarmSettingsTable.FindAsync(1).AsTask();

        public Task<FarmSettings> ActiveFarmSettings => FarmSettingsTable.FindAsync(2).AsTask();
        
        public DbSet<FarmSettings> FarmSettingsTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ForFarmDbContext).Assembly);

            //initialize settings and state
            modelBuilder.Entity<FarmSettings>().HasData(new FarmSettings {ID = 1}, new FarmSettings {ID = 2});
            modelBuilder.Entity<FarmState>().HasData(new FarmState());

            modelBuilder.Entity<Account>()
                .HasOne<Instance>()
                .WithOne(a => a.Account)
                .HasForeignKey<Instance>(i => i.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Proxy)
                .WithMany(p => p.ActiveAccounts)
                .HasForeignKey(a => a.ProxyId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Bot>()
                .HasOne<MuleRequest>()
                .WithOne(r => r.Bot)
                .HasForeignKey<MuleRequest>(r => r.BotId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Mule>()
                .HasMany<MuleRequest>()
                .WithOne(r => r.Mule)
                .HasForeignKey(m => m.MuleId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
