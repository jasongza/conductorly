using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    public interface IQueryHandler<in TRequest, TResponse>
        where TRequest : IQuery<TResponse>
    {
        Task<TResponse> Handle(TRequest query);
    }
}
