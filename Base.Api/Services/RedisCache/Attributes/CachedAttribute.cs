using Base.Application.Services.RedisCache.Extensions;
using Base.Application.Services.RedisCache.Models.Config;
using Base.Application.Services.RedisCache.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace Base.API.Services.RedisCache.Attributes;

/// <summary>
/// cache attribute
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CachedAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _timeToLiveSeconds; // period of the cache in sec

    /// <summary>
    /// 
    /// </summary>
    /// <param name="timeToLiveSeconds"></param>
    public CachedAttribute(int timeToLiveSeconds)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        config.GetSection("RedisCache").Get<RedisConfig>();
        var redisConfig = config.GetRedisConfig();

        // fetch the cancellation token from the request
        var cancellationToken = context.HttpContext.RequestAborted;

        // fetch the cache settings from app settings of the application or microservice
        if (!redisConfig
                .Enabled) // the cache is not enabled so the request should continue executing normally without any cache consideration
        {
            await next();
            return;
        }

        // initiate the cache service
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

        // generate the cache key
        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

        // fetch the cache response
        var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey, cancellationToken);

        // the cache is not empty. so the response should be created from the redis cache
        if (!string.IsNullOrEmpty(cachedResponse))
        {
            var contentResult = new ContentResult()
            {
                Content = cachedResponse,
                ContentType = "application/json",
                StatusCode = 200
            };

            context.Result = contentResult;
            return;
        }

        // go to the endpoint because the cache is empty. 
        var executedContext = await next();

        // fetching the result after going to the endpoint and cache it only if the response is ok
        if (executedContext.Result is OkObjectResult okObjectResult)
        {
            // set the response into redis cache
            await cacheService.CacheResponseAsync(cacheKey, JsonConvert.SerializeObject(okObjectResult.Value),
                TimeSpan.FromSeconds(_timeToLiveSeconds), cancellationToken);
        }
    }


    /// <summary>
    /// generate cache key per requests.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append($"{request.Path}");
        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"{key} - {value}");
        }

        return keyBuilder.ToString();
    }
}