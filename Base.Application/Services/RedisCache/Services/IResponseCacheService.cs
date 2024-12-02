namespace Base.Application.Services.RedisCache.Services;

/// <summary>
/// Defines a service for caching and retrieving responses using Redis
/// </summary>
public interface IResponseCacheService
{
    /// <summary>
    /// Caches a response in Redis with the specified key and time-to-live
    /// </summary>
    /// <param name="cacheKey">The unique key to identify the cached response</param>
    /// <param name="response">The response object to cache</param>
    /// <param name="timeToLive">The duration for which the response should remain cached</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    /// <remarks>
    /// If the response is null, it will still be cached to indicate a null result.
    /// The timeToLive parameter determines how long the response will be available in the cache.
    /// </remarks>
    Task CacheResponseAsync(string cacheKey, object? response, TimeSpan timeToLive,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a cached response from Redis by its key
    /// </summary>
    /// <param name="cacheKey">The unique key of the cached response</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>The cached response as a string, or null if not found</returns>
    /// <remarks>
    /// The response is returned as a string and needs to be deserialized by the caller.
    /// Returns null if the key does not exist in the cache.
    /// </remarks>
    Task<string?> GetCachedResponseAsync(string cacheKey, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves and deserializes a cached response from Redis
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to</typeparam>
    /// <param name="cacheKey">The unique key of the cached response</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>The deserialized cached response, or default(T) if not found</returns>
    /// <remarks>
    /// This method handles the deserialization of the cached response to the specified type.
    /// Returns default(T) if the key does not exist in the cache or if deserialization fails.
    /// </remarks>
    Task<T?> GetCachedResponseAsync<T>(string cacheKey, CancellationToken cancellationToken) where T : class;

    /// <summary>
    /// Refreshes the expiration time of a cached response
    /// </summary>
    /// <param name="cacheKey">The unique key of the cached response</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    /// <remarks>
    /// This operation is not recommended for regular use as it can lead to cache inconsistency.
    /// Use only when absolutely necessary and understand the implications.
    /// </remarks>
    Task RefreshCacheResponseAsync(string cacheKey, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a cached response from Redis
    /// </summary>
    /// <param name="cacheKey">The unique key of the cached response to remove</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    /// <remarks>
    /// If the key is null or does not exist, the operation completes successfully without error.
    /// </remarks>
    Task ResetCacheResponseAsync(string? cacheKey, CancellationToken cancellationToken);

    /// <summary>
    /// Removes multiple cached responses matching a pattern
    /// </summary>
    /// <param name="pattern">The pattern to match cache keys against (e.g., "user:*")</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>The number of cache entries removed</returns>
    /// <remarks>
    /// This operation can be expensive on large datasets. Use with caution.
    /// Pattern matching follows Redis KEYS command syntax.
    /// </remarks>
    Task<int> ResetCacheByPatternAsync(string pattern, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if a response exists in the cache
    /// </summary>
    /// <param name="cacheKey">The unique key to check</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>True if the key exists in the cache, false otherwise</returns>
    Task<bool> ExistsAsync(string cacheKey, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the time-to-live for a cached response
    /// </summary>
    /// <param name="cacheKey">The unique key of the cached response</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>The remaining time-to-live, or null if the key does not exist</returns>
    Task<TimeSpan?> GetTimeToLiveAsync(string cacheKey, CancellationToken cancellationToken);
}