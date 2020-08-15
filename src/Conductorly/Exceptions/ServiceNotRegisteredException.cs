using System;

namespace Conductorly.Exceptions
{
    /// <summary>
    /// Represents errors that occur when services are not registered.
    /// </summary>
    public class ServiceNotRegisteredException : Exception
    {
        public ServiceNotRegisteredException(string message) : base(message)
        {
        }
    }
}
