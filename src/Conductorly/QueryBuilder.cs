using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Conductorly
{
    /// <summary>
    /// Default Conductorly <see cref="IQueryBuilder{TRequest, TResponse}"/>.
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    /// <typeparam name="TResponse">Response Type</typeparam>
    public class QueryBuilder<TRequest, TResponse> : ConductorlyBase, IQueryBuilder<TRequest, TResponse>
        where TRequest : IQuery<TResponse>
    {
        private readonly TRequest query;

        private IQueryHandler<TRequest, TResponse> currentHandler;

        /// <summary>
        /// Initializes <see cref="QueryBuilder{TRequest, TResponse}"/>.
        /// </summary>
        /// <param name="query">Query Type</param>
        /// <param name="scopeFactory"><see cref="IServiceScopeFactory"/></param>
        public QueryBuilder(TRequest query, IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            this.query = query;

            currentHandler = new DecoratorQueryHandler<TRequest, TResponse>((query, next) => InvokeHandle(query));
        }

        /// <summary>
        /// Decorate the existing <see cref="IQueryHandler{TRequest, TResponse}"/>.
        /// </summary>
        /// <param name="function">Function</param>
        /// <returns><see cref="IQueryBuilder{TRequest, TResponse}"/></returns>
        public IQueryBuilder<TRequest, TResponse> Decorate(Func<TRequest, IQueryHandler<TRequest, TResponse>, Task<TResponse>> function)
        {
            currentHandler = new DecoratorQueryHandler<TRequest, TResponse>(function, currentHandler);

            return this;
        }

        /// <summary>
        /// Start the builder workflow.
        /// </summary>
        /// <returns><see cref="Task{TResponse}"/></returns>
        public Task<TResponse> Start()
        {
            return currentHandler.Handle(query);
        }
    }
}
