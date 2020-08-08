using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    public interface ICommandDecorator<TRequest> where TRequest : ICommand
    {
        Task Send(TRequest command);
    }
}