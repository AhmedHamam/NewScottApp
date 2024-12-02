using Base.Application.Interfaces;
using Base.Application.Services.RedisCache.Extensions;
using Base.Application.Services.RedisCache.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that handles cache invalidation for commands
/// </summary>
/// <typeparam name="TRequest">The type of request being handled</typeparam>
/// <typeparam name="TResponse">The type of response from the handler</typeparam>
/// <remarks>
/// This behavior automatically invalidates related cache entries when a command
/// that implements IResetCacheCommand is executed. It follows the convention that
/// commands and their related queries are in parallel namespaces (e.g., 
/// "Domain.Commands.UpdateUser" would invalidate cache for "Domain.Queries.*")
/// </remarks>
public class ResetCacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the ResetCacheBehavior class
    /// </summary>
    /// <param name="httpContextAccessor">HTTP context accessor for accessing request-scoped services</param>
    /// <param name="configuration">Application configuration for Redis settings</param>
    public ResetCacheBehavior(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Handles the request by executing cache invalidation if applicable
    /// </summary>
    /// <param name="request">The incoming request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="next">Delegate to the next handler in the pipeline</param>
    /// <returns>The response from the next handler</returns>
    /// <remarks>
    /// The cache invalidation process:
    /// 1. Checks if Redis is enabled and if the request implements IResetCacheCommand
    /// 2. Executes the request
    /// 3. Invalidates related cache entries based on the request type
    /// </remarks>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        // Skip cache reset if Redis is disabled or request doesn't implement IResetCacheCommand
        if (!_configuration.GetRedisConfig().Enabled || 
            !typeof(IResetCacheCommand).IsAssignableFrom(request.GetType()))
        {
            return await next();
        }

        var cacheService = _httpContextAccessor.HttpContext?.RequestServices
            .GetRequiredService<IResponseCacheService>();

        if (cacheService == null)
            return await next();

        // Execute the request first
        var response = await next();

        // Reset cache after successful execution
        var key = GetCacheKeyPrefix(request);
        if (!string.IsNullOrEmpty(key))
        {
            await cacheService.ResetCacheResponseAsync(key, cancellationToken);
        }

        return response;
    }

    /// <summary>
    /// Generates a cache key prefix based on the request type's namespace
    /// </summary>
    /// <param name="request">The request to generate a key for</param>
    /// <returns>A string key prefix for cache invalidation</returns>
    /// <remarks>
    /// Converts command namespace to related query namespace.
    /// Example: "Domain.Commands.UpdateUser" -> "Domain.Queries."
    /// </remarks>
    private static string? GetCacheKeyPrefix(TRequest request)
    {
        var fullName = request.GetType().FullName;
        return fullName?.Split(".Commands.")[0] + ".Queries.";
    }
}