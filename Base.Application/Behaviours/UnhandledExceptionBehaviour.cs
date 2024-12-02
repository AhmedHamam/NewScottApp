using MediatR;
using Serilog;

namespace Base.Application.Behaviours;

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
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
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

            Log.Error(ex, 
                "Unhandled Exception for Request {RequestName} ({RequestType}) {@Request}",
                requestName,
                requestType,
                request);

            // Re-throw the original exception to maintain the stack trace
            throw;
        }
    }
}