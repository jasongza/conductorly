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
                await conductorly.Send
                (
                    new PrintCommand(await conductorly.Send(new HelloQuery("World")))
                );

                await Task.Delay(1000, stoppingToken);

                Console.WriteLine($"================= START DECORATED QUERY ================= ");

                // Simplify usage...
                var response = await conductorly.With<HelloQuery, string>(new HelloQuery("Chained"))
                    .Decorate(async (query, next) =>
                    {
                        var stopWatch = Stopwatch.StartNew();
                        var result = await next.Handle(query);

                        Console.WriteLine($"Handler diagnostics: {stopWatch.ElapsedMilliseconds}ms");

                        return result;
                    })
                    .Decorate(async (query, next) =>
                    {
                        Console.WriteLine($"Before...");
                        var result = await next.Handle(query);
                        Console.WriteLine($"After...");

                        return result;
                    })
                    .Send();

                Console.WriteLine(response);

                Console.WriteLine($"================= END DECORATED QUERY ================= ");

                await Task.Delay(1000, stoppingToken);

                Console.WriteLine($"================= START DECORATED COMMAND ================= ");

                await conductorly.With(new PrintCommand("I'm chained!"))
                    .Decorate(async (command, next) => 
                    {
                        var stopWatch = Stopwatch.StartNew();

                        await next.Handle(command);

                        Console.WriteLine($"Handler diagnostics: {stopWatch.ElapsedMilliseconds}ms");
                    })
                    .Decorate(async (command, next) =>
                    {
                        Console.WriteLine($"Before...");

                        await next.Handle(command);

                        Console.WriteLine($"After...");
                    })
                    .Send();

                Console.WriteLine($"================= END DECORATED COMMAND ================= ");

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
