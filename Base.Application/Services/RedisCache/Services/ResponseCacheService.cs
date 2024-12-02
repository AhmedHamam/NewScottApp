using Base.Application.Services.RedisCache.Extensions;
using Base.Application.Services.RedisCache.Models.Config;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Base.Application.Services.RedisCache.Services;

/// <summary>
/// Implementation of IResponseCacheService using Redis distributed cache
/// </summary>
public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly RedisConfig _config;
    private readonly ILogger<ResponseCacheService> _logger;
    private readonly JsonSerializerSettings _jsonSettings;

    /// <summary>
    /// Initializes a new instance of the ResponseCacheService
    /// </summary>
    public ResponseCacheService(
        IDistributedCache distributedCache,
        IConfiguration configuration,
        ILogger<ResponseCacheService> logger)
    {
        _distributedCache = distributedCache;
        _config = configuration.GetRedisConfig();
        _logger = logger;
        _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
    }

    /// <inheritdoc/>
    public async Task CacheResponseAsync(string cacheKey, object? response, TimeSpan timeToLive,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!_config.Enabled || string.IsNullOrWhiteSpace(cacheKey))
            {
                return;
            }

            var serializedResponse = JsonConvert.SerializeObject(response, _jsonSettings);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            };

            await _distributedCache.SetStringAsync(cacheKey, serializedResponse, options, cancellationToken);
            _logger.LogDebug("Cached response for key: {CacheKey}", cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error caching response for key: {CacheKey}", cacheKey);
            if (_config.ThrowOnError) throw;
        }
    }

    /// <inheritdoc/>
    public async Task<string?> GetCachedResponseAsync(string cacheKey, CancellationToken cancellationToken)
    {
        try
        {
            if (!_config.Enabled || string.IsNullOrWhiteSpace(cacheKey))
            {
                return null;
            }

            var cachedResponse = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
            return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving cached response for key: {CacheKey}", cacheKey);
            if (_config.ThrowOnError) throw;
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<T?> GetCachedResponseAsync<T>(string cacheKey, CancellationToken cancellationToken) where T : class
    {
        try
        {
            var cachedResponse = await GetCachedResponseAsync(cacheKey, cancellationToken);
            if (string.IsNullOrEmpty(cachedResponse))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(cachedResponse, _jsonSettings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deserializing cached response for key: {CacheKey}", cacheKey);
            if (_config.ThrowOnError) throw;
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task RefreshCacheResponseAsync(string cacheKey, CancellationToken cancellationToken)
    {
        try
        {
            if (!_config.Enabled || string.IsNullOrWhiteSpace(cacheKey))
            {
                return;
            }

            await _distributedCache.RefreshAsync(cacheKey, cancellationToken);
            _logger.LogDebug("Refreshed cache for key: {CacheKey}", cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing cache for key: {CacheKey}", cacheKey);
            if (_config.ThrowOnError) throw;
        }
    }

    /// <inheritdoc/>
    public async Task ResetCacheResponseAsync(string? cacheKey, CancellationToken cancellationToken)
    {
        try
        {
            if (!_config.Enabled || string.IsNullOrWhiteSpace(cacheKey))
            {
                return;
            }

            await _distributedCache.RemoveAsync(cacheKey, cancellationToken);
            _logger.LogDebug("Reset cache for key: {CacheKey}", cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting cache for key: {CacheKey}", cacheKey);
            if (_config.ThrowOnError) throw;
        }
    }

    /// <inheritdoc/>
    public async Task<int> ResetCacheByPatternAsync(string pattern, CancellationToken cancellationToken)
    {
        try
        {
            if (!_config.Enabled || string.IsNullOrWhiteSpace(pattern) || string.IsNullOrWhiteSpace(_config.ConnectionString))
            {
                return 0;
            }

            using var redis = await ConnectionMultiplexer.ConnectAsync(_config.ConnectionString);
            var server = redis.GetServer(GetHostName(_config.ConnectionString), GetHostPort(_config.ConnectionString));
            
            var count = 0;
            await foreach (var key in server.KeysAsync(0, pattern, 1000).WithCancellation(cancellationToken))
            {
                await _distributedCache.RemoveAsync(key, cancellationToken);
                count++;
            }

            _logger.LogDebug("Reset {Count} cache entries matching pattern: {Pattern}", count, pattern);
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting cache for pattern: {Pattern}", pattern);
            if (_config.ThrowOnError) throw;
            return 0;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string cacheKey, CancellationToken cancellationToken)
    {
        try
        {
            if (!_config.Enabled || string.IsNullOrWhiteSpace(cacheKey))
            {
                return false;
            }

            var value = await _distributedCache.GetAsync(cacheKey, cancellationToken);
            return value != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence for key: {CacheKey}", cacheKey);
            if (_config.ThrowOnError) throw;
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<TimeSpan?> GetTimeToLiveAsync(string cacheKey, CancellationToken cancellationToken)
    {
        try
        {
            if (!_config.Enabled || string.IsNullOrWhiteSpace(cacheKey) || string.IsNullOrWhiteSpace(_config.ConnectionString))
            {
                return null;
            }

            using var redis = await ConnectionMultiplexer.ConnectAsync(_config.ConnectionString);
            var db = redis.GetDatabase();
            var ttl = await db.KeyTimeToLiveAsync(cacheKey);
            return ttl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting TTL for key: {CacheKey}", cacheKey);
            if (_config.ThrowOnError) throw;
            return null;
        }
    }

    private static string GetHostName(string connectionString)
        => connectionString.Split(":").First();

    private static int GetHostPort(string connectionString)
        => connectionString.Split(":").Length > 1
           && connectionString.Split(":")[1].All(char.IsDigit)
            ? int.Parse(connectionString.Split(":")[1])
            : 6379;
}