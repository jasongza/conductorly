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

        private ICommandDecorator<TRequest> currentAction;

        public CommandBuilder(TRequest command, IServiceScopeFactory scopeFactory) : base(scopeFactory)
        {
            this.command = command;

            currentAction = new DefaultCommandDecorator<TRequest>((command, next) => Send(command));
        }

        public ICommandBuilder<TRequest> Decorate(Func<TRequest, ICommandDecorator<TRequest>, Task> action)
        {
            currentAction = new DefaultCommandDecorator<TRequest>(action, currentAction);

            return this;
        }

        public Task Send()
        {
            return currentAction.Send(command);
        }
    }
}
