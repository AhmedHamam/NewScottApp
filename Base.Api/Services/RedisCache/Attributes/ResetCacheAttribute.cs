using Base.Application.Services.RedisCache.Extensions;
using Base.Application.Services.RedisCache.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.API.Services.RedisCache.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ResetCacheAttribute : Attribute, IAsyncResultFilter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var redisConfig = config.GetRedisConfig();

        // fetch the cancellation token from the request
        var cancellationToken = context.HttpContext.RequestAborted;

        if (!redisConfig
                .Enabled) // the cache is not enabled so the request should continue executing normally without any cache consideration
        {
            await next();
            return;
        }

        var resetCacheFilter = context.Filters.Where(e => e.GetType() == typeof(ResetCacheAttribute));
        if (resetCacheFilter.Any())
        {
            var controllerName = context.HttpContext.Request.Path;
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            await cacheService.ResetCacheResponseAsync(controllerName, cancellationToken);
            await next();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MethodInfo MethodOf(Expression<Action> expression)
    {
        MethodCallExpression body = (MethodCallExpression)expression.Body;
        return body.Method;
    }

    private static bool MethodHasResetCacheAttribute(Expression<Action> expression)
    {
        var method = MethodOf(expression);

        const bool includeInherited = false;
        return method.GetCustomAttributes(typeof(ResetCacheAttribute), includeInherited).Any();
    }
}