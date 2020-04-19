using FORFarm.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, bool development)
        {
            if (development)
            {
                services.AddDbContext<ForFarmDbContext>(options => options.UseSqlite("Filename=MyDatabase.db"));
            }
            else
            {
                services.AddDbContext<ForFarmDbContext>(
                    options => options.UseSqlServer(configuration.GetConnectionString("Azure")));
            }
            
            services.AddScoped<IFarmContext>(s => s.GetRequiredService<ForFarmDbContext>());
            
            return services;
        }
    }
}