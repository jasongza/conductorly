using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Conductorly
{
    /// <summary>
    /// Default Conductorly <see cref="ICommandBuilder{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    public class CommandBuilder<TRequest> : ConductorlyBase, ICommandBuilder<TRequest>
        where TRequest : ICommand
    {
        private readonly TRequest command;

        private ICommandHandler<TRequest> currentHandler;

        /// <summary>
        /// Initializes <see cref="CommandBuilder{TRequest}"/>.
        /// </summary>
        /// <param name="command">Request Type</param>
        /// <param name="scopeFactory"><see cref="IServiceScopeFactory"/></param>
        public CommandBuilder(TRequest command, IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            this.command = command;

            currentHandler = new DecoratorCommandHandler<TRequest>((command, next) => InvokeHandle(command));
        }

        /// <summary>
        /// Decorate the existing <see cref="ICommandHandler{TRequest}"/>.
        /// </summary>
        /// <param name="function">Function</param>
        /// <returns><see cref="ICommandBuilder{TRequest}"/></returns>
        public ICommandBuilder<TRequest> Decorate(Func<TRequest, ICommandHandler<TRequest>, Task> function)
        {
            currentHandler = new DecoratorCommandHandler<TRequest>(function, currentHandler);

            return this;
        }

        /// <summary>
        /// Start the builder workflow.
        /// </summary>
        /// <returns><see cref="Task"/></returns>
        public Task Start()
        {
            return currentHandler.Handle(command);
        }
    }
}
