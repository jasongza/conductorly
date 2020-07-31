using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    public interface IConductorly
    {
        Task<TResponse> Send<TResponse>(IQuery<TResponse> query);

        Task Send(ICommand command);
    }
}
