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

        private IQueryDecorator<TRequest, TResponse> currentAction;

        public QueryBuilder(TRequest query, IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            this.query = query;

            currentAction = new DefaultQueryDecorator<TRequest, TResponse>((query, next) => Send(query));
        }

        public IQueryBuilder<TRequest, TResponse> Decorate(Func<TRequest, IQueryDecorator<TRequest, TResponse>, Task<TResponse>> function)
        {
            currentAction = new DefaultQueryDecorator<TRequest, TResponse>(function, currentAction);

            return this;
        }

        public Task<TResponse> Send()
        {
            return currentAction.Send(query);
        }
    }
}
