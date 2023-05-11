using Base.API.Services.Swagger.Extensions;
using Base.API.Services.Swagger.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Base.API.Services.Swagger;

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
    public static IServiceCollection AddBaseSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSwaggerConfig();

        if (!config.Enabled) return services;
        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(x => x.FullName);
            c.OperationFilter<SwaggerFileOperationFilter>();
            c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });
            c.AddAuthorizationWithJwt();
        });
        services.ConfigureOptions<ConfigureSwaggerOptions>();

        return services;
    }
}