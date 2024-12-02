namespace Base.Application.Interfaces;

/// <summary>
/// Defines a command that can reset cache entries
/// </summary>
public interface IResetCacheCommand
{
    /// <summary>
    /// Gets the cache keys that should be reset when this command is executed
    /// </summary>
    /// <returns>An array of cache keys to reset</returns>
    string[] CacheKeys { get; }

    /// <summary>
    /// Gets a value indicating whether to reset all cache entries
    /// </summary>
    bool ResetAllCacheEntries { get; }

    /// <summary>
    /// Gets the pattern to match cache keys that should be reset
    /// </summary>
    string? CacheKeyPattern { get; }
}