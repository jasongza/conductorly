using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    public interface ICommandDecorator<in TRequest> where TRequest : ICommand
    {
        Task Send(TRequest command);
    }
}