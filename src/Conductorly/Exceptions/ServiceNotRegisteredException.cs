using System;

namespace Conductorly.Exceptions
{
    public class ServiceNotRegisteredException : Exception
    {
        public ServiceNotRegisteredException(string message) : base(message)
        {
        }
    }
}
