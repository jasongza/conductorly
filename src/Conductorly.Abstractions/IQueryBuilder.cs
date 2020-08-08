using System;
using System.Threading.Tasks;

namespace Conductorly.Abstractions
{
    public interface IQueryBuilder<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        IQueryBuilder<TRequest, TResponse> Decorate(Func<TRequest, IQueryHandler<TRequest, TResponse>, Task<TResponse>> function);
        Task<TResponse> Start();
    }
}
