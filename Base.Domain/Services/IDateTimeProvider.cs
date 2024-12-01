namespace Base.Domain.Services
{
    /// <summary>
    /// Defines a service interface for providing consistent date and time values
    /// </summary>
    /// <remarks>
    /// This interface abstracts the system clock to facilitate testing and ensure
    /// consistent time handling across the application. It's particularly useful
    /// for audit trails, domain events, and any time-dependent operations.
    /// </remarks>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Gets the current UTC date and time
        /// </summary>
        /// <returns>The current UTC date and time</returns>
        DateTime UtcNow { get; }
    }
}
