using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    /// <summary>
    /// Defines a Conductorly query handler.
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    /// <typeparam name="TResponse">Response Type</typeparam>
    public interface IQueryHandler<in TRequest, TResponse>
        where TRequest : IQuery<TResponse>
    {
        /// <summary>
        /// Handle incoming request and returns <see cref="Task{TResponse}"/>.
        /// </summary>
        /// <param name="query">Request Type</param>
        /// <returns><see cref="Task{TResponse}"/></returns>
        Task<TResponse> Handle(TRequest query);
    }
}
