using Conductorly.Abstractions;
using System;
using System.Threading.Tasks;

namespace Conductorly
{
    /// Represents an instance of <see cref="IQueryHandler{TRequest, TResponse}"/> used in <see cref="QueryBuilder{TRequest, TResponse}"/>.
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    /// <typeparam name="TResponse">Response Type</typeparam>
    internal class DecoratorQueryHandler<TRequest, TResponse> : IQueryHandler<TRequest, TResponse>
        where TRequest: IQuery<TResponse>
    {
        private readonly Func<TRequest, IQueryHandler<TRequest, TResponse>, Task<TResponse>> function;
        private readonly IQueryHandler<TRequest, TResponse> next;

        public DecoratorQueryHandler(Func<TRequest, IQueryHandler<TRequest, TResponse>, Task<TResponse>> function, IQueryHandler<TRequest, TResponse> next)
        {
            this.function = function;
            this.next = next;
        }

        public DecoratorQueryHandler(Func<TRequest, IQueryHandler<TRequest, TResponse>, Task<TResponse>> function)
        {
            this.function = function;
        }

        public Task<TResponse> Handle(TRequest query)
        {
            return function(query, next);
        }
    }
}
