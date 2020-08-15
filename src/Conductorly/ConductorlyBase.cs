using Conductorly.Abstractions;
using Conductorly.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Conductorly
{
    /// <summary>
    /// Shared Conductorly base class.
    /// </summary>
    public abstract class ConductorlyBase
    {
        private readonly IServiceScopeFactory scopeFactory;

        public ConductorlyBase(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Find and invoke the registered handler for the query.
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="query">Request Type</param>
        /// <returns><see cref="Task{TResponse}"/></returns>
        /// <exception cref="ServiceNotRegisteredException">Thrown when IQueryHandler for <paramref name="query"/> is not registered.</exception>
        protected Task<TResponse> InvokeHandle<TResponse>(IQuery<TResponse> query)
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

        /// <summary>
        /// Find and invoke the registered handler for the command
        /// </summary>
        /// <param name="command">Request Type</param>
        /// <returns><see cref="Task"/></returns>
        /// <exception cref="ServiceNotRegisteredException">Thrown when ICommandHandler for <paramref name="command"/> is not registered.</exception>
        protected Task InvokeHandle(ICommand command)
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