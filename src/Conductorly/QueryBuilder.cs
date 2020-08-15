using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Conductorly
{
    public class QueryBuilder<TRequest, TResponse> : ConductorlyBase, IQueryBuilder<TRequest, TResponse>
        where TRequest : IQuery<TResponse>
    {
        private readonly TRequest query;

        private IQueryHandler<TRequest, TResponse> currentHandler;

        public QueryBuilder(TRequest query, IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            this.query = query;

            currentHandler = new DecoratorQueryHandler<TRequest, TResponse>((query, next) => InvokeHandle(query));
        }

        public IQueryBuilder<TRequest, TResponse> Decorate(Func<TRequest, IQueryHandler<TRequest, TResponse>, Task<TResponse>> function)
        {
            currentHandler = new DecoratorQueryHandler<TRequest, TResponse>(function, currentHandler);

            return this;
        }

        public Task<TResponse> Start()
        {
            return currentHandler.Handle(query);
        }
    }
}
