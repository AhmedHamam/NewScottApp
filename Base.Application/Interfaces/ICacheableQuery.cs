namespace Base.Application.Interfaces;

/// <summary>
/// Defines a query that can be cached
/// </summary>
public interface ICacheableQuery
{
    /// <summary>
    /// Gets a unique cache key for the query
    /// </summary>
    /// <returns>A string that uniquely identifies the query for caching</returns>
    string CacheKey { get; }

    /// <summary>
    /// Gets the sliding expiration time for the cache entry
    /// </summary>
    TimeSpan? SlidingExpiration { get; }

    /// <summary>
    /// Gets the absolute expiration time for the cache entry
    /// </summary>
    DateTimeOffset? AbsoluteExpiration { get; }
}