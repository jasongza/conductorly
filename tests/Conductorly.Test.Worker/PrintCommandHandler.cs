using Conductorly.Abstractions;
using System;
using System.Threading.Tasks;

namespace Conductorly.Test.Worker
{
    public class PrintCommandHandler : ICommandHandler<PrintCommand>
    {
        public Task Handle(PrintCommand request)
        {
            Console.WriteLine($"Command: {request.Message}");

            return Task.CompletedTask;
        }
    }
}
