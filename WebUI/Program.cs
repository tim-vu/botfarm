using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Seeding.SeedAccounts;
using FORFarm.Application.Seeding.SeedProxies;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbContext = services.GetRequiredService<ForFarmDbContext>();
                    
                    dbContext.Database.Migrate();

                    if (!dbContext.Accounts.Any())
                    {
                        var mediator = services.GetRequiredService<IMediator>();
                        mediator.Send(new SeedAccounts());
                        mediator.Send(new SeedProxies());
                    }
                }
                catch (Exception ex)
                {
                    // ignored
                }
            }
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}