using Conductorly.Abstractions;
using System;
using System.Threading.Tasks;

namespace Conductorly
{
    /// <summary>
    /// Represents an instance of <see cref="ICommandHandler{TRequest}"/> used in <see cref="CommandBuilder{TRequest}"/>.
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    internal class DecoratorCommandHandler<TRequest> : ICommandHandler<TRequest>
        where TRequest: ICommand
    {
        private readonly Func<TRequest, ICommandHandler<TRequest>, Task> function;
        private readonly ICommandHandler<TRequest> next;

        public DecoratorCommandHandler(Func<TRequest, ICommandHandler<TRequest>, Task> function, ICommandHandler<TRequest> next)
        {
            this.function = function;
            this.next = next;
        }

        public DecoratorCommandHandler(Func<TRequest, ICommandHandler<TRequest>, Task> function)
        {
            this.function = function;
        }

        public Task Handle(TRequest command)
        {
            return function(command, next);
        }
    }
}
