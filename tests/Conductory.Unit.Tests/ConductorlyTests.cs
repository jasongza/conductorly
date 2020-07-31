using Conductorly;
using Conductorly.Abstractions;
using Conductorly.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Conductory.Unit.Tests
{
    public class ConductorlyTests
    {
        [Fact]
        public async void Send_WithRegisteredQuery_InvokesExpectedMethod()
        {
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            var mockServiceScope = new Mock<IServiceScope>();

            mockServiceScopeFactory.Setup(mock => mock.CreateScope())
                .Returns(mockServiceScope.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();

            mockServiceScope.Setup(mock => mock.ServiceProvider)
                .Returns(mockServiceProvider.Object);

            var query = new Mock<IQuery<string>>();

            var mockQueryHandler = new Mock<IQueryHandler<IQuery<string>, string>>();

            mockQueryHandler.Setup(mock => mock.Handle(query.Object))
                .Returns(Task.FromResult("success!"));

            mockServiceProvider.Setup(mock => mock.GetService(It.IsAny<Type>()))
                .Returns(mockQueryHandler.Object);

            var conductorly = new Conductorly.Conductorly(mockServiceScopeFactory.Object);

            var result = await conductorly.Send(query.Object);

            Assert.Equal("success!", result);
        }

        [Fact]
        public void Send_WithUnknownQuery_ThrowsServiceNotRegisteredException()
        {
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            var mockServiceScope = new Mock<IServiceScope>();

            mockServiceScopeFactory.Setup(mock => mock.CreateScope())
                .Returns(mockServiceScope.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();

            mockServiceScope.Setup(mock => mock.ServiceProvider)
                .Returns(mockServiceProvider.Object);

            var query = new Mock<IQuery<string>>();

            var conductorly = new Conductorly.Conductorly(mockServiceScopeFactory.Object);

            Assert.Throws<ServiceNotRegisteredException>(() => 
            { 
                conductorly.Send(query.Object).Wait(); 
            });
        }

        [Fact]
        public async void Send_WithRegisteredCommand_InvokesExpectedMethod()
        {
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            var mockServiceScope = new Mock<IServiceScope>();

            mockServiceScopeFactory.Setup(mock => mock.CreateScope())
                .Returns(mockServiceScope.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();

            mockServiceScope.Setup(mock => mock.ServiceProvider)
                .Returns(mockServiceProvider.Object);

            var command = new Mock<ICommand>();

            var mockCommandHandler = new Mock<ICommandHandler<ICommand>>();

            mockCommandHandler.Setup(mock => mock.Handle(command.Object))
                .Returns(Task.CompletedTask);

            mockServiceProvider.Setup(mock => mock.GetService(It.IsAny<Type>()))
                .Returns(mockCommandHandler.Object);

            var conductorly = new Conductorly.Conductorly(mockServiceScopeFactory.Object);

            await conductorly.Send(command.Object);

            mockCommandHandler.Verify(mock => mock.Handle(command.Object), Times.Once);
        }

        [Fact]
        public void Send_WithUnknownCommand_ThrowsServiceNotRegisteredException()
        {
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            var mockServiceScope = new Mock<IServiceScope>();

            mockServiceScopeFactory.Setup(mock => mock.CreateScope())
                .Returns(mockServiceScope.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();

            mockServiceScope.Setup(mock => mock.ServiceProvider)
                .Returns(mockServiceProvider.Object);

            var command = new Mock<ICommand>();

            var conductorly = new Conductorly.Conductorly(mockServiceScopeFactory.Object);

            Assert.Throws<ServiceNotRegisteredException>(() => 
            { 
                conductorly.Send(command.Object).Wait(); 
            });
        }
    }
}
