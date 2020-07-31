using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Conductorly.Test.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseConductorly()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<ICommandHandler<PrintCommand>, PrintCommandHandler>();
                    services.AddScoped<IQueryHandler<HelloQuery, string>, HelloQueryHandler>();

                    services.AddHostedService<Worker>();
                });
    }
}
