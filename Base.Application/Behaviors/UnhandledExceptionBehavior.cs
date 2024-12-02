using MediatR;
using Microsoft.Extensions.Logging;

namespace Base.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that handles unhandled exceptions in the request pipeline
/// </summary>
/// <typeparam name="TRequest">The type of request being handled</typeparam>
/// <typeparam name="TResponse">The type of response from the handler</typeparam>
/// <remarks>
/// This behavior ensures that all unhandled exceptions are properly logged
/// before being re-thrown, maintaining the original stack trace while providing
/// detailed logging information for debugging purposes.
/// </remarks>
public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of the UnhandledExceptionBehavior
    /// </summary>
    /// <param name="logger">The logger instance for this behavior</param>
    public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the request and catches any unhandled exceptions
    /// </summary>
    /// <param name="request">The incoming request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="next">Delegate to the next handler in the pipeline</param>
    /// <returns>The response from the next handler</returns>
    /// <exception cref="Exception">Re-throws any caught exception after logging</exception>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            var requestType = typeof(TRequest).FullName;

            _logger.LogError(ex,
                "Application Request: Unhandled Exception for Request {Name} ({Type})\nException: {Exception}\nRequest: {@Request}",
                requestName,
                requestType,
                ex.Message,
                request);

            // Re-throw the original exception to maintain the stack trace
            throw;
        }
    }
}