using System;
using System.Threading;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FORFarm.Application.Farm.FarmManager
{
    public class FarmTaskExecutor : IHostedService, IDisposable
    {
        private static readonly TimeSpan ExecuteInterval = TimeSpan.FromSeconds(1);
        
        private Timer _timer;
        private readonly ILogger<FarmTaskExecutor> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public FarmTaskExecutor(ILogger<FarmTaskExecutor> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecuteFarmAction, null, ExecuteInterval, ExecuteInterval);

            return Task.CompletedTask;
        }

        private async void ExecuteFarmAction(object argument)
        {
            using var scope = _scopeFactory.CreateScope();
            
            var farmService = scope.ServiceProvider.GetRequiredService<IFarmService>();

            var action = await farmService.GetFarmAction();

            _logger.LogDebug($"Executing farmAction: {action.GetType().Name}");

            await action.Execute();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}