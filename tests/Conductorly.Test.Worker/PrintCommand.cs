using Conductorly.Abstractions;

namespace Conductorly.Test.Worker
{
    public class PrintCommand : ICommand
    {
        public PrintCommand(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
