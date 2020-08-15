using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Conductorly
{
    /// <summary>
    /// Default Conductorly implementation, the dispatching service.
    /// </summary>
    public sealed class Conductorly : ConductorlyBase, IConductorly
    {
        private readonly IServiceScopeFactory scopeFactory;

        /// <summary>
        /// Initializes <see cref="Conductorly"/>
        /// </summary>
        /// <param name="scopeFactory"><see cref="IServiceScopeFactory"/></param>
        public Conductorly(IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Starts the decorator workflow and returns <see cref="ICommandBuilder{TRequest}"/>.
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <param name="command">Request Type</param>
        /// <returns><see cref="ICommandBuilder{TRequest}"/></returns>
        public ICommandBuilder<TRequest> With<TRequest>(TRequest command) where TRequest : ICommand
        {
            return new CommandBuilder<TRequest>(command, scopeFactory);
        }


        /// <summary>
        /// Starts the decorator workflow and returns <see cref="IQueryBuilder{TRequest, TResponse}"/>.
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="query">Request Type</param>
        /// <returns><see cref="IQueryBuilder{TRequest, TResponse}"/></returns>
        public IQueryBuilder<TRequest, TResponse> With<TRequest, TResponse>(TRequest query) where TRequest : IQuery<TResponse>
        {
            return new QueryBuilder<TRequest, TResponse>(query, scopeFactory);
        }

        /// <summary>
        /// Send the query and returns <see cref="Task{TResponse}"/>.
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="query">Request Type</param>
        /// <returns><see cref="Task{TResponse}"/></returns>
        public Task<TResponse> Send<TResponse>(IQuery<TResponse> query)
        {
            return InvokeHandle(query);
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="command">Request Type</param>
        /// <returns><see cref="Task"/></returns>
        public Task Send(ICommand command)
        {
            return InvokeHandle(command);
        }
    }
}
