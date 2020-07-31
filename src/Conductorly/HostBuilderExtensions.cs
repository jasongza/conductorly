using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Conductorly
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseConductorly(this IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IConductorly, Conductorly>();
            });

            return builder;
        }
    }
}
