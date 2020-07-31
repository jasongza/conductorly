using Conductorly.Abstractions;
using System.Threading.Tasks;

namespace Conductorly.Test.Worker
{
    public class HelloQueryHandler : IQueryHandler<HelloQuery, string>
    {
        public Task<string> Handle(HelloQuery query)
        {
            return Task.FromResult($"Hello {query.Name}!");
        }
    }
}
