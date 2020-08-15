using Conductorly;
using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Conductory.Unit.Tests
{
    public class CommandBuilderTests
    {
        [Fact]
        public async void Start_WithDecorations_ReturnsExpectedResult()
        {
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            var mockServiceScope = new Mock<IServiceScope>();

            mockServiceScopeFactory.Setup(mock => mock.CreateScope())
                .Returns(mockServiceScope.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();

            mockServiceScope.Setup(mock => mock.ServiceProvider)
                .Returns(mockServiceProvider.Object);

            var command = new TestCommand
            {
                Counter = 0
            };

            var mockCommandHandler = new Mock<ICommandHandler<ICommand>>();

            mockCommandHandler.Setup(mock => mock.Handle(command))
                .Returns(Task.CompletedTask);

            mockServiceProvider.Setup(mock => mock.GetService(It.IsAny<Type>()))
                .Returns(mockCommandHandler.Object);

            var builder = new CommandBuilder<TestCommand>(command, mockServiceScopeFactory.Object);

            await builder.Decorate(async (command, next) =>
            {
                command.Counter *= 5;

                await next.Handle(command);
            })
            .Decorate(async (command, next) =>
            {
                command.Counter += 2;

                await next.Handle(command);
            })
            .Start();

            mockCommandHandler.Verify(mock => mock.Handle(command), Times.Once);
            Assert.Equal(10, command.Counter);
        }

        private class TestCommand : ICommand
        {
            public int Counter { get; set; }
        }
    }
    public class QueryBuilderTests
    {
        [Fact]
        public async void Start_WithDecorations_ReturnsExpectedResult()
        {
            var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            var mockServiceScope = new Mock<IServiceScope>();

            mockServiceScopeFactory.Setup(mock => mock.CreateScope())
                .Returns(mockServiceScope.Object);

            var mockServiceProvider = new Mock<IServiceProvider>();

            mockServiceScope.Setup(mock => mock.ServiceProvider)
                .Returns(mockServiceProvider.Object);

            var query = new TestQuery
            {
                Counter = 0
            };

            var mockQueryHandler = new Mock<IQueryHandler<IQuery<string>, string>>();

            mockQueryHandler.Setup(mock => mock.Handle(query))
                .Returns(Task.FromResult("query result!"));

            mockServiceProvider.Setup(mock => mock.GetService(It.IsAny<Type>()))
                .Returns(mockQueryHandler.Object);

            var builder = new QueryBuilder<TestQuery, string>(query, mockServiceScopeFactory.Object);

            var result = await builder.Decorate((query, next) =>
            {
                query.Counter *= 5;

                return next.Handle(query);
            })
            .Decorate((query, next) =>
            {
                query.Counter += 2;

                return next.Handle(query);
            })
            .Start();

            mockQueryHandler.Verify(mock => mock.Handle(query), Times.Once);
            Assert.Equal(10, query.Counter);
            Assert.Equal("query result!", result);
        }

        private class TestQuery : IQuery<string>
        {
            public int Counter { get; set; }
        }
    }
}
