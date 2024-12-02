namespace Base.Application.Services.RedisCache.Models.Config;

/// <summary>
/// Configuration settings for Redis caching service
/// </summary>
public sealed class RedisConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether Redis caching is enabled
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// Gets or sets the Redis connection string
    /// Format: "localhost:6379" or "redis-server:6379,password=xxx,ssl=True"
    /// </summary>
    /// <remarks>
    /// Required when <see cref="Enabled"/> is true.
    /// For SSL connections, include ssl=True in the connection string.
    /// For password-protected instances, include password=xxx in the connection string.
    /// </remarks>
    public string? ConnectionString { get; init; }

    /// <summary>
    /// Gets or sets the default cache expiration time in minutes
    /// </summary>
    /// <remarks>
    /// Defaults to 60 minutes if not specified.
    /// Set to -1 for no expiration.
    /// </remarks>
    public int DefaultExpirationMinutes { get; init; } = 60;

    /// <summary>
    /// Gets or sets the maximum retry attempts for Redis operations
    /// </summary>
    /// <remarks>
    /// Defaults to 3 attempts if not specified.
    /// </remarks>
    public int MaxRetryAttempts { get; init; } = 3;

    /// <summary>
    /// Gets or sets the retry delay in milliseconds between attempts
    /// </summary>
    /// <remarks>
    /// Defaults to 1000ms (1 second) if not specified.
    /// </remarks>
    public int RetryDelayMilliseconds { get; init; } = 1000;

    /// <summary>
    /// Gets or sets a value indicating whether to throw exceptions on Redis failures
    /// </summary>
    /// <remarks>
    /// If false, failures will be logged but not thrown.
    /// Defaults to false if not specified.
    /// </remarks>
    public bool ThrowOnError { get; init; }

    /// <summary>
    /// Validates the configuration settings
    /// </summary>
    /// <returns>True if the configuration is valid, false otherwise</returns>
    public bool IsValid()
    {
        if (!Enabled) return true;
        return !string.IsNullOrWhiteSpace(ConnectionString);
    }

    /// <summary>
    /// Creates a new instance of RedisConfig with default settings
    /// </summary>
    /// <returns>A new RedisConfig instance</returns>
    public static RedisConfig CreateDefault() => new()
    {
        Enabled = false,
        DefaultExpirationMinutes = 60,
        MaxRetryAttempts = 3,
        RetryDelayMilliseconds = 1000,
        ThrowOnError = false
    };
}