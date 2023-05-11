namespace Base.Application.Services.RedisCache.Services;

/// <summary>
/// 
/// </summary>
public interface IResponseCacheService
{
    /// <summary>
    /// set the response inside redis cache
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="response"></param>
    /// <param name="timeToLive"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CacheResponseAsync(string cacheKey, object? response, TimeSpan timeToLive,
        CancellationToken cancellationToken);


    /// <summary>
    /// get the cache value from redis cache by key
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> GetCachedResponseAsync(string cacheKey, CancellationToken cancellationToken);

    /// <summary>
    /// refresh redis cache. its not recommended to use it.
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RefreshCacheResponseAsync(string cacheKey, CancellationToken cancellationToken);

    /// <summary>
    /// delete redis cache by key
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ResetCacheResponseAsync(string? cacheKey, CancellationToken cancellationToken);
}