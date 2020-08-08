using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    public interface IQueryDecorator<in TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        Task<TResponse> Send(TRequest query);
    }
}