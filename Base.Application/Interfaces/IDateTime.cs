namespace Base.Application.Interfaces;

/// <summary>
/// Provides access to the current date and time
/// </summary>
public interface IDateTime
{
    /// <summary>
    /// Gets the current date and time in UTC
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current date and time in the local time zone
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Gets today's date in the local time zone
    /// </summary>
    DateTime Today { get; }

    /// <summary>
    /// Gets the current date and time as DateTimeOffset in UTC
    /// </summary>
    DateTimeOffset UtcNowOffset { get; }

    /// <summary>
    /// Gets the current date and time as DateTimeOffset in the local time zone
    /// </summary>
    DateTimeOffset NowOffset { get; }
}