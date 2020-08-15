using System;
using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    /// <summary>
    /// Defines a Conductorly query builder.
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    /// <typeparam name="TResponse">Response Type</typeparam>
    public interface IQueryBuilder<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        /// <summary>
        /// Decorate the query.
        /// </summary>
        /// <param name="function">Function</param>
        /// <returns><see cref="IQueryBuilder{TRequest, TResponse}"/></returns>
        IQueryBuilder<TRequest, TResponse> Decorate(Func<TRequest, IQueryHandler<TRequest, TResponse>, Task<TResponse>> function);

        /// <summary>
        /// Start the builder workflow.
        /// </summary>
        /// <returns><see cref="Task{TResponse}"/></returns>
        Task<TResponse> Start();
    }
}
