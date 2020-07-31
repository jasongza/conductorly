using Conductorly;
using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Conductory.Unit.Tests
{
    public class HostBuilderExtensionsTests
    {
        [Fact]
        public void UseConductorly_BindsExpectedImplementations()
        {
            var mockServiceCollection = new Mock<IServiceCollection>();
            var hostBuilder = new TestHostBuilder(mockServiceCollection.Object);

            HostBuilderExtensions.UseConductorly(hostBuilder);

            mockServiceCollection.Verify(mock => mock.Add(
                It.Is<ServiceDescriptor>(descriptor => 
                    descriptor.ServiceType == typeof(IConductorly) &&
                    descriptor.ImplementationType == typeof(Conductorly.Conductorly) &&
                    descriptor.Lifetime == ServiceLifetime.Singleton)), Times.Once);
        }
    }
}
