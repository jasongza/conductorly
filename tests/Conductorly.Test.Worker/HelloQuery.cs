using Conductorly.Abstractions;

namespace Conductorly.Test.Worker
{
    public class HelloQuery : IQuery<string>
    {
        public HelloQuery(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}