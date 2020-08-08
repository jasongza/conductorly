using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    public interface IConductorly
    {
        Task<TResponse> Send<TResponse>(IQuery<TResponse> query);

        Task Send(ICommand command);

        ICommandBuilder<TRequest> WithCommand<TRequest>(TRequest command) where TRequest : ICommand;

        IQueryBuilder<TRequest, TResponse> WithQuery<TRequest, TResponse>(TRequest query) where TRequest : IQuery<TResponse>;
    }
}
