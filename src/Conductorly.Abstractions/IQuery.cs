namespace Conductorly.Abstractions
{
    /// <summary>
    /// Defines a Conductory query with an expected response type.
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    public interface IQuery<out TResponse>
    {
    }
}
