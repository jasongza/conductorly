using Conductorly.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
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
                var response = await conductorly.Send(new HelloQuery("World"));

                await conductorly.Send(new PrintCommand(response));

                Console.WriteLine($"=================");

                await conductorly.With(new PrintCommand("I'm chained!"))
                    .Decorate(async (command, next) => 
                    {
                        var stopWatch = Stopwatch.StartNew();

                        await next.Send(command);

                        Console.WriteLine($"Handler diagnostics: {stopWatch.ElapsedMilliseconds}ms");
                    })
                    .Decorate(async (command, next) =>
                    {
                        Console.WriteLine($"Before...");

                        await next.Send(command);

                        Console.WriteLine($"After...");
                    })
                    .Send();

                Console.WriteLine($"=================");

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
