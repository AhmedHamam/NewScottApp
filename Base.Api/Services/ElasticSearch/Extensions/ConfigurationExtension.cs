using Base.API.Services.ElasticSearch.Models.Config;
using Microsoft.Extensions.Configuration;

namespace Base.API.Services.ElasticSearch.Extensions;

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
    public static ElasticSearchConfig GetElasticSearchConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection("ElasticSearch").Get<ElasticSearchConfig>();
        if (config is null)
        {
            throw new Exception("Missing 'ElasticSearch' configuration section from the appsettings.");
        }

        return config;
    }
}