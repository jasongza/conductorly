using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Conductorly
{
    /// <summary>
    /// Conductorly IHostBuilder extension methods.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds Conductorly services to the container.
        /// </summary>
        /// <param name="builder"><see cref="IHostBuilder"/></param>
        /// <returns><see cref="IHostBuilder"/></returns>
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
