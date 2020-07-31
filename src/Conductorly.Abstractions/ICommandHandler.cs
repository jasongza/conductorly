using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    public interface ICommandHandler<in TRequest>
        where TRequest : ICommand
    {
        Task Handle(TRequest command);
    }
}