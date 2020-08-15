using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    /// <summary>
    /// Defines Conductorly, the dispatching service.
    /// </summary>
    public interface IConductorly
    {
        /// <summary>
        /// Send the query and returns <see cref="Task{TResponse}"/>.
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="query">Request Type</param>
        /// <returns><see cref="Task{TResponse}"/></returns>
        Task<TResponse> Send<TResponse>(IQuery<TResponse> query);

        /// <summary>
        /// Send the command.
        /// </summary>
        /// <param name="command">Request Type</param>
        /// <returns><see cref="Task"/></returns>
        Task Send(ICommand command);

        /// <summary>
        /// Starts the decorator workflow and returns <see cref="ICommandBuilder{TRequest}"/>.
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <param name="command">Request Type</param>
        /// <returns><see cref="ICommandBuilder{TRequest}"/></returns>
        ICommandBuilder<TRequest> With<TRequest>(TRequest command) where TRequest : ICommand;

        /// <summary>
        /// Starts the decorator workflow and returns <see cref="IQueryBuilder{TRequest, TResponse}"/>.
        /// </summary>
        /// <typeparam name="TRequest">Request Type</typeparam>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="query">Request Type</param>
        /// <returns><see cref="IQueryBuilder{TRequest, TResponse}"/></returns>
        IQueryBuilder<TRequest, TResponse> With<TRequest, TResponse>(TRequest query) where TRequest : IQuery<TResponse>;
    }
}
