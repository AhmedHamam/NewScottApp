using Base.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Base.API.Services.ExceptionHandling;

/// <summary>
/// 
/// </summary>
public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            // _logger.LogError(e, e.Message);

            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        int statusCode;
        object response;
        switch (exception)
        {
            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                response = new
                {
                    title = "Unauthorized",
                    status = statusCode,
                    detail = exception.Message
                };
                break;
            case ForbiddenAccessException:
                statusCode = StatusCodes.Status403Forbidden;
                response = new
                {
                    title = "Forbidden",
                    status = statusCode,
                    detail = exception.Message
                };
                break;
            case NotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                response = new
                {
                    title = "Not found",
                    status = statusCode,
                    detail = exception.Message
                };
                break;
            case ValidationException:
                statusCode = StatusCodes.Status400BadRequest;
                response = new
                {
                    title = "Validation Errors",
                    status = statusCode,
                    detail = exception.Message,
                    errors = GetErrors(exception)
                };
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                response = new
                {
                    title = "Internal server error",
                    status = statusCode,
                    detail = exception.Message,
                };
                break;
        }


        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        //await httpContext.Response.CompleteAsync();
        //httpContext.Features.Get<IConnectionLifetimeFeature>()?.Abort();
        // httpContext.Abort();
    }

    private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
    {
        IReadOnlyDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        if (exception is ValidationException validationException)
        {
            errors = validationException.ErrorsDictionary;
        }

        return errors;
    }
}