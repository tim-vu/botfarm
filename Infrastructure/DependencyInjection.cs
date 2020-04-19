using FORFarm.Application.Common.Interfaces;
using Infrastructure.RSPeerApi;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IDateTime, MachineDateTime>();
            services.AddSingleton<IRandom, MachineRandom>();
            services.AddTransient<IClientHandler, RsPeerApiClient>();
            services.AddHttpClient();
        }
    }
}