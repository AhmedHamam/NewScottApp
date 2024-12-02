using Base.Application.Interfaces;
using Base.Application.Services.RedisCache.Constants;
using Base.Application.Services.RedisCache.Extensions;
using Base.Application.Services.RedisCache.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Base.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that implements caching for query requests
/// </summary>
/// <typeparam name="TRequest">The type of request being handled</typeparam>
/// <typeparam name="TResponse">The type of response from the handler</typeparam>
public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the CachingBehavior class
    /// </summary>
    /// <param name="httpContextAccessor">HTTP context accessor for accessing request-scoped services</param>
    /// <param name="configuration">Application configuration for Redis settings</param>
    public CachingBehavior(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Handles the request by implementing caching logic
    /// </summary>
    /// <param name="request">The incoming request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="next">Delegate to the next handler in the pipeline</param>
    /// <returns>The response from cache or the next handler</returns>
    /// <remarks>
    /// The caching logic follows these steps:
    /// 1. Check if Redis caching is enabled in configuration
    /// 2. Verify if the request implements ICacheableQuery
    /// 3. Try to get cached response using a key generated from request properties
    /// 4. If cache miss, execute the request and cache the response
    /// </remarks>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        // Skip caching if Redis is not enabled
        if (!_configuration.GetRedisConfig().Enabled)
            return await next();

        var cacheService = _httpContextAccessor.HttpContext?.RequestServices
            .GetRequiredService<IResponseCacheService>();

        if (cacheService == null)
            return await next();

        // Only cache requests that implement ICacheableQuery
        var requestType = request.GetType();
        if (!typeof(ICacheableQuery).IsAssignableFrom(requestType))
            return await next();

        var key = GetKey(request);
        
        // Try to get from cache first
        var cachedResponse = await cacheService.GetCachedResponseAsync(key, cancellationToken);
        if (!string.IsNullOrEmpty(cachedResponse))
            return JsonConvert.DeserializeObject<TResponse>(cachedResponse) 
                ?? throw new InvalidOperationException("Failed to deserialize cached response");

        // Cache miss - execute request and cache response
        var response = await next();
        await cacheService.CacheResponseAsync(key, response,
            TimeSpan.FromSeconds(CacheSpan.Day), cancellationToken);

        return response;
    }

    /// <summary>
    /// Generates a unique cache key based on the request type and its properties
    /// </summary>
    /// <param name="request">The request to generate a key for</param>
    /// <returns>A string key that uniquely identifies this request</returns>
    private string GetKey(TRequest request)
    {
        var properties = JsonConvert.SerializeObject(request.GetType().GetProperties()
            .Select(p => $"{p.Name}:{GetPropertyValue(request, p.Name)}"));
        return $"{request.GetType().FullName}: {properties}";
    }

    /// <summary>
    /// Gets the value of a property from an object using reflection
    /// </summary>
    /// <param name="obj">The object to get the property value from</param>
    /// <param name="propertyName">Name of the property to get</param>
    /// <returns>The value of the property</returns>
    private static object? GetPropertyValue(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
    }
}