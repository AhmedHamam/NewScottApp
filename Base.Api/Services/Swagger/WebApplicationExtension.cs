using Base.API.Services.Swagger.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Base.API.Services.Swagger;

public static class WebApplicationExtension
{
    public static WebApplication UseBaseSwagger(this WebApplication app, IConfiguration configuration)
    {
        var config = configuration.GetSwaggerConfig();

        if (!config.Enabled) return app;
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            var depth = config.HideModels ? -1 : 1;
            c.DefaultModelsExpandDepth(depth); // Disable swagger schemas at bottom
            // c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        });

        return app;
    }
}