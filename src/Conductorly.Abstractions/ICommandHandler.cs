using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    /// <summary>
    /// Defines a Conductorly command handler.
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    public interface ICommandHandler<in TRequest>
        where TRequest : ICommand
    {
        /// <summary>
        /// Handle incoming request.
        /// </summary>
        /// <param name="command">Request Type</param>
        /// <returns><see cref="Task"/></returns>
        Task Handle(TRequest command);
    }
}