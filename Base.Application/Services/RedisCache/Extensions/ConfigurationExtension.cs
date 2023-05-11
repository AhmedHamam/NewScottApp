using Base.Application.Services.RedisCache.Models.Config;
using Microsoft.Extensions.Configuration;

namespace Base.Application.Services.RedisCache.Extensions;

/// <summary>
/// 
/// </summary>
public static class ConfigurationExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static RedisConfig GetRedisConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection("RedisCache").Get<RedisConfig>();
        if (config is null)
        {
            throw new Exception("Missing 'RedisCache' configuration section from the appsettings.");
        }

        return config;
    }
}