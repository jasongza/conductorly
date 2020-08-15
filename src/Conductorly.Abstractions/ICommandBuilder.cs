using System;
using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    /// <summary>
    /// Defines a Conductorly command builder.
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    public interface ICommandBuilder<TRequest> where TRequest : ICommand
    {
        /// <summary>
        /// Decorate the command.
        /// </summary>
        /// <param name="function">Function</param>
        /// <returns><see cref="ICommandBuilder{TRequest}"/></returns>
        ICommandBuilder<TRequest> Decorate(Func<TRequest, ICommandHandler<TRequest>, Task> function);

        /// <summary>
        /// Start the builder workflow.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        Task Start();
    }
}
