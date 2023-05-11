using Base.Application.Interfaces;
using Base.Application.Services.RedisCache.Constants;
using Base.Application.Services.RedisCache.Extensions;
using Base.Application.Services.RedisCache.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Base.Application.Behaviours;

public class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public CachingBehaviour(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (!_configuration.GetRedisConfig().Enabled)
            return await next();

        var cacheService =
            _httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<IResponseCacheService>();

        var requestType = request.GetType();
        if (!typeof(ICacheableQuery).IsAssignableFrom(requestType)) return await next();

        var key = GetKey(request);
        var cachedResponse = await cacheService!.GetCachedResponseAsync(key, cancellationToken);
        if (!string.IsNullOrEmpty(cachedResponse))
            return JsonConvert.DeserializeObject<TResponse>(cachedResponse);

        var response = await next();
        await cacheService.CacheResponseAsync(key, response,
            TimeSpan.FromSeconds(CacheSpan.Day), cancellationToken);

        return response;
    }

    private string GetKey(TRequest request)
    {
        var properties = JsonConvert.SerializeObject(request.GetType().GetProperties()
            .Select(p => $"{p.Name}:{GetValObjDy(request, p.Name)}"));
        return $"{request.GetType().FullName}: {properties}";
    }

    private object? GetValObjDy(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
    }
}