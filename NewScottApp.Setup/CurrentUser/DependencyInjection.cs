using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace NewScotApp.Setup.CurrentUser;

public static class DependencyInjection
{
    /// <summary>
    /// HttpContextAccessor must be registered in program.cs
    /// builder.Services.AddHttpContextAccessor();
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
    {
        CurrentUser.InitializeHttpContextAccessor(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        return app;
    }
}