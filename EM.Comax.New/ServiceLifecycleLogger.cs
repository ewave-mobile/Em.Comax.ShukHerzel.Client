// Services/ServiceLifecycleLogger.cs
using EM.Comax.ShukHerzel.Models.Interfaces;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace EM.Comax.New.WorkerService.Services
{
    public class ServiceLifecycleLogger : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceLifecycleLogger(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("ServiceLifecycleLogger: Worker Service is starting.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<IDatabaseLogger>();
                await logger.LogServiceActionAsync("Worker Service started.");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information("ServiceLifecycleLogger: Worker Service is stopping.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<IDatabaseLogger>();
                await logger.LogServiceActionAsync("Worker Service stopped.");
            }
        }
    }
}
