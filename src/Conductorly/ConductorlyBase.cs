using Conductorly.Abstractions;
using Conductorly.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Conductorly
{
    public abstract class ConductorlyBase
    {
        private readonly IServiceScopeFactory scopeFactory;

        public ConductorlyBase(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public Task<TResponse> Send<TResponse>(IQuery<TResponse> query)
        {
            using var scope = scopeFactory.CreateScope();

            var requestType = query.GetType();
            var responseType = typeof(TResponse);
            var type = typeof(IQueryHandler<,>).MakeGenericType(requestType, responseType);
            var handler = scope.ServiceProvider.GetService(type);

            if (handler == null)
            {
                throw new ServiceNotRegisteredException($"Service IQueryHandler<{requestType.Name}, {responseType.Name}> not registered.");
            }

            var methodInfo = type.GetMethod("Handle");

            return methodInfo.Invoke(handler, new[] { query }) as Task<TResponse>;
        }

        public Task Send(ICommand command)
        {
            using var scope = scopeFactory.CreateScope();

            var requestType = command.GetType();
            var type = typeof(ICommandHandler<>).MakeGenericType(requestType);
            var handler = scope.ServiceProvider.GetService(type);

            if (handler == null)
            {
                throw new ServiceNotRegisteredException($"Service ICommandHandler<{requestType.Name}> not registered.");
            }

            var methodInfo = type.GetMethod("Handle");

            return methodInfo.Invoke(handler, new[] { command }) as Task;
        }
    }
}