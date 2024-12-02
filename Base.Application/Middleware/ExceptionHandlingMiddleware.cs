using Base.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using UnauthorizedAccessException = Base.Application.Exceptions.UnauthorizedAccessException;
using ValidationException = FluentValidation.ValidationException;

namespace Base.Application.Middleware;

/// <summary>
/// Middleware for handling exceptions globally across the application
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred while executing request: {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            TraceId = context.TraceIdentifier,
            Timestamp = DateTime.UtcNow
        };

        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Title = "Validation Error";
                errorResponse.Detail = "One or more validation errors occurred.";
                errorResponse.Errors = validationEx.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );
                break;

            case NotFoundException notFoundEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Title = "Resource Not Found";
                errorResponse.Detail = notFoundEx.Message;
                break;

            case UnauthorizedAccessException unauthorizedEx:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Title = "Unauthorized";
                errorResponse.Detail = unauthorizedEx.Message;
                break;

            case ForbiddenAccessException forbiddenEx:
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Title = "Forbidden";
                errorResponse.Detail = forbiddenEx.Message;
                break;

            case BadRequestException badRequestEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Title = "Bad Request";
                errorResponse.Detail = badRequestEx.Message;
                if (!string.IsNullOrEmpty(badRequestEx.PropertyName))
                {
                    errorResponse.Errors = new Dictionary<string, string[]>
                    {
                        { badRequestEx.PropertyName, new[] { badRequestEx.Reason ?? badRequestEx.Message } }
                    };
                }
                break;

            case InvalidOperationException invalidOpEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Title = "Invalid Operation";
                errorResponse.Detail = invalidOpEx.Message;
                break;

            case TimeoutException timeoutEx:
                response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Title = "Request Timeout";
                errorResponse.Detail = timeoutEx.Message;
                break;

            case OperationCanceledException canceledEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Title = "Operation Canceled";
                errorResponse.Detail = canceledEx.Message;
                break;

            case ArgumentException argEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Title = "Invalid Argument";
                errorResponse.Detail = argEx.Message;
                errorResponse.Errors = new Dictionary<string, string[]>
                {
                    { argEx.ParamName ?? "Parameter", new[] { argEx.Message } }
                };
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Title = "Internal Server Error";
                errorResponse.Detail = _environment.IsDevelopment()
                    ? exception.Message
                    : "An unexpected error occurred while processing your request.";

                if (_environment.IsDevelopment())
                {
                    errorResponse.DevelopmentDetails = new DevelopmentErrorDetails
                    {
                        ExceptionType = exception.GetType().FullName,
                        StackTrace = exception.StackTrace,
                        InnerException = exception.InnerException?.Message
                    };
                }
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse, JsonOptions);
        await response.WriteAsync(result);
    }
}
