using Conductorly.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Conductorly.Test.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConductorly conductorly;

        public Worker(ILogger<Worker> logger, IConductorly conductorly)
        {
            _logger = logger;
            this.conductorly = conductorly;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var response = await conductorly.Send(new HelloQuery("World"));

                await conductorly.Send(new PrintCommand(response));

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
