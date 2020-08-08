using Conductorly.Abstractions;
using System;
using System.Threading.Tasks;

namespace Conductorly
{
    internal class DefaultQueryDecorator<TRequest, TResponse> : IQueryDecorator<TRequest, TResponse>
        where TRequest: IQuery<TResponse>
    {
        private readonly Func<TRequest, IQueryDecorator<TRequest, TResponse>, Task<TResponse>> function;
        private readonly IQueryDecorator<TRequest, TResponse> next;

        public DefaultQueryDecorator(Func<TRequest, IQueryDecorator<TRequest, TResponse>, Task<TResponse>> function, IQueryDecorator<TRequest, TResponse> next)
        {
            this.function = function;
            this.next = next;
        }

        public DefaultQueryDecorator(Func<TRequest, IQueryDecorator<TRequest, TResponse>, Task<TResponse>> function)
        {
            this.function = function;
        }

        public async Task<TResponse> Send(TRequest query)
        {
            return await function(query, next);
        }
    }
}
