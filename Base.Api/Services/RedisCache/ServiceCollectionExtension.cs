using Base.Application.Services.RedisCache.Extensions;
using Base.Application.Services.RedisCache.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Base.API.Services.RedisCache;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetRedisConfig();

        if (config.Enabled)
        {
            services.AddStackExchangeRedisCache(options => options.Configuration = config.ConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }

        return services;
    }
}