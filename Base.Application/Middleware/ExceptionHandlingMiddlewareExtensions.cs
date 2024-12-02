using Microsoft.AspNetCore.Builder;

namespace Base.Application.Middleware;

/// <summary>
/// Extension method to add the exception handling middleware to the application pipeline
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
