using Conductorly.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Conductorly
{
    public class CommandBuilder<TRequest> : ConductorlyBase, ICommandBuilder<TRequest>
        where TRequest : ICommand
    {
        private readonly TRequest command;

        private ICommandHandler<TRequest> currentHandler;

        public CommandBuilder(TRequest command, IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            this.command = command;

            currentHandler = new DecoratorCommandHandler<TRequest>((command, next) => Send(command));
        }

        public ICommandBuilder<TRequest> Decorate(Func<TRequest, ICommandHandler<TRequest>, Task> function)
        {
            currentHandler = new DecoratorCommandHandler<TRequest>(function, currentHandler);

            return this;
        }

        public Task Start()
        {
            return currentHandler.Handle(command);
        }
    }
}
