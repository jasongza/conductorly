using System;
using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    public interface ICommandBuilder<TRequest> where TRequest : ICommand
    {
        ICommandBuilder<TRequest> Decorate(Func<TRequest, ICommandDecorator<TRequest>, Task> action);
        Task Send();
    }
}
