using Conductorly.Abstractions;
using System;
using System.Threading.Tasks;

namespace Conductorly
{
    internal class DefaultCommandDecorator<TRequest> : ICommandDecorator<TRequest>
        where TRequest: ICommand
    {
        private readonly Func<TRequest, ICommandDecorator<TRequest>, Task> action;
        private readonly ICommandDecorator<TRequest> next;

        public DefaultCommandDecorator(Func<TRequest, ICommandDecorator<TRequest>, Task> action, ICommandDecorator<TRequest> next)
        {
            this.action = action;
            this.next = next;
        }

        public DefaultCommandDecorator(Func<TRequest, ICommandDecorator<TRequest>, Task> action)
        {
            this.action = action;
        }

        public async Task Send(TRequest command)
        {
            await action(command, next);
        }
    }
}
