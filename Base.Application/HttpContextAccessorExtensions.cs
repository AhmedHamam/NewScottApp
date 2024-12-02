using Base.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using UnauthorizedAccessException = Base.Application.Exceptions.UnauthorizedAccessException;

namespace Base.Application;

/// <summary>
/// Extension methods for IHttpContextAccessor to handle common HTTP response scenarios
/// </summary>
public static class HttpContextAccessorExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    /// <summary>
    /// Sends a 404 Not Found response and aborts the request pipeline
    /// </summary>
    /// <param name="accessor">The HTTP context accessor</param>
    /// <param name="message">Optional message describing why the resource was not found</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <exception cref="NotFoundException">Thrown when the response has already started or context is null</exception>
    public static async Task SendNotFoundAndAbort(this IHttpContextAccessor? accessor, string? message)
    {
        var response = new
        {
            title = "Not Found",
            status = StatusCodes.Status404NotFound,
            detail = message,
            timestamp = DateTime.UtcNow
        };

        if (accessor?.HttpContext == null || accessor.HttpContext.Response.HasStarted)
            throw new NotFoundException(JsonSerializer.Serialize(response, JsonOptions));

        await accessor.HttpContext.SetResponseAndAbort(StatusCodes.Status404NotFound,
            JsonSerializer.Serialize(response, JsonOptions));
    }

    /// <summary>
    /// Sends a 401 Unauthorized response and aborts the request pipeline
    /// </summary>
    /// <param name="accessor">The HTTP context accessor</param>
    /// <param name="message">Optional message describing why the request was unauthorized</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the response has already started or context is null</exception>
    public static async Task SendUnauthorizedAndAbort(this IHttpContextAccessor? accessor, string? message)
    {
        var response = new
        {
            title = "Unauthorized",
            status = StatusCodes.Status401Unauthorized,
            detail = message,
            timestamp = DateTime.UtcNow
        };

        if (accessor?.HttpContext == null || accessor.HttpContext.Response.HasStarted)
            throw new UnauthorizedAccessException(JsonSerializer.Serialize(response, JsonOptions));

        await accessor.HttpContext.SetResponseAndAbort(StatusCodes.Status401Unauthorized,
            JsonSerializer.Serialize(response, JsonOptions));
    }

    /// <summary>
    /// Sends a 403 Forbidden response and aborts the request pipeline
    /// </summary>
    /// <param name="accessor">The HTTP context accessor</param>
    /// <param name="message">Optional message describing why the request was forbidden</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the response has already started or context is null</exception>
    public static async Task SendForbiddenAndAbort(this IHttpContextAccessor? accessor, string? message)
    {
        var response = new
        {
            title = "Forbidden",
            status = StatusCodes.Status403Forbidden,
            detail = message,
            timestamp = DateTime.UtcNow
        };

        if (accessor?.HttpContext == null || accessor.HttpContext.Response.HasStarted)
            throw new ForbiddenAccessException(JsonSerializer.Serialize(response, JsonOptions));

        await accessor.HttpContext.SetResponseAndAbort(StatusCodes.Status403Forbidden,
            JsonSerializer.Serialize(response, JsonOptions));
    }

    /// <summary>
    /// Sends a 400 Bad Request response and aborts the request pipeline
    /// </summary>
    /// <param name="accessor">The HTTP context accessor</param>
    /// <param name="message">Message describing why the request was invalid</param>
    /// <param name="errors">Optional dictionary of validation errors</param>
    /// <returns>A task representing the asynchronous operation</returns>
    /// <exception cref="ValidationException">Thrown when the response has already started or context is null</exception>
    public static async Task SendBadRequestAndAbort(this IHttpContextAccessor? accessor, string message,
        Dictionary<string, string[]>? errors = default)
    {
        var response = new
        {
            title = "Bad Request",
            status = StatusCodes.Status400BadRequest,
            detail = message,
            errors = errors ?? new Dictionary<string, string[]>(),
            timestamp = DateTime.UtcNow
        };

        if (accessor?.HttpContext == null || accessor.HttpContext.Response.HasStarted)
            throw new ValidationException(errors ?? new Dictionary<string, string[]>());

        await accessor.HttpContext.SetResponseAndAbort(StatusCodes.Status400BadRequest,
            JsonSerializer.Serialize(response, JsonOptions));
    }

    /// <summary>
    /// Sends a custom response with the specified status code and aborts the request pipeline
    /// </summary>
    /// <param name="accessor">The HTTP context accessor</param>
    /// <param name="statusCode">The HTTP status code to send</param>
    /// <param name="response">Optional response object to serialize</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public static async Task SendResponseAndAbort(this IHttpContextAccessor? accessor, int statusCode,
        object? response = default)
    {
        if (accessor?.HttpContext == null || accessor.HttpContext.Response.HasStarted)
            return;

        var wrappedResponse = response == null ? null : new
        {
            status = statusCode,
            data = response,
            timestamp = DateTime.UtcNow
        };

        await accessor.HttpContext.SetResponseAndAbort(statusCode,
            JsonSerializer.Serialize(wrappedResponse, JsonOptions));
    }

    /// <summary>
    /// Sets the response status code, content type, and writes the message to the response body
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="statusCode">The HTTP status code to set</param>
    /// <param name="message">The message to write to the response body</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private static async Task SetResponseAndAbort(this HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(message, context.RequestAborted);
        await context.Response.CompleteAsync();
    }
}