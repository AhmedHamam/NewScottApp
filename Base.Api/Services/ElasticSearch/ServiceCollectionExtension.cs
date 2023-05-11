using Base.API.Services.ElasticSearch.Extensions;
using Base.API.Variables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Base.API.Services.ElasticSearch;

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
    public static IServiceCollection AddElasticSearch(this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetElasticSearchConfig();
        var configurationRoot = (IConfigurationRoot)configuration;

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(ConfigureElasticSink(config.Url!))
            .Enrich.WithProperty("Environment", EnvVariables.ENVIRONMENT_NAME)
            .ReadFrom.Configuration(configurationRoot)
            .CreateLogger();

        return services;
    }

    private static ElasticsearchSinkOptions ConfigureElasticSink(string url)
    {
        return new ElasticsearchSinkOptions(new Uri(url))
        {
            AutoRegisterTemplate = true,
            IndexFormat =
                $"{Assembly.GetEntryAssembly()?.GetName().Name?.ToLower().Replace(".", "-")}-{EnvVariables.ENVIRONMENT_NAME?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        };
    }
}