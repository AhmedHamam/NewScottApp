using Base.Application.Interfaces;
using Base.Application.Services.RedisCache.Extensions;
using Base.Application.Services.RedisCache.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Application.Behaviours;

public class ResetCacheBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public ResetCacheBehaviour(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (!_configuration.GetRedisConfig().Enabled || !typeof(IResetCacheCommand).IsAssignableFrom(request.GetType()))
            return await next();

        var cacheService = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
        var response = await next();
        var key = GetKey(request);
        await cacheService.ResetCacheResponseAsync(key, cancellationToken);
        return response;
    }

    private string GetKey(TRequest request)
        => request.GetType().FullName?.Split(".Commands.")[0] + ".Queries.";
}