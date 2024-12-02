using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;

namespace Base.Application.Behaviours;

/// <summary>
/// MediatR pipeline behavior that monitors request execution time and logs slow requests
/// </summary>
/// <typeparam name="TRequest">The type of request being handled</typeparam>
/// <typeparam name="TResponse">The type of response from the handler</typeparam>
/// <remarks>
/// This behavior tracks the execution time of each request and logs a warning
/// when requests take longer than the configured threshold (default: 500ms).
/// </remarks>
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private const int LongRunningThresholdMs = 500;

    /// <summary>
    /// Initializes a new instance of the PerformanceBehaviour class
    /// </summary>
    /// <param name="logger">Logger instance for the request type</param>
    public PerformanceBehaviour(ILogger<TRequest> logger)
    {
        _timer = new Stopwatch();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the request while monitoring its execution time
    /// </summary>
    /// <param name="request">The incoming request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="next">Delegate to the next handler in the pipeline</param>
    /// <returns>The response from the next handler</returns>
    /// <remarks>
    /// If the request takes longer than LongRunningThresholdMs (500ms by default),
    /// a warning will be logged with request details and execution time.
    /// </remarks>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        _timer.Start();

        try
        {
            return await next();
        }
        finally
        {
            _timer.Stop();
            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > LongRunningThresholdMs)
            {
                var requestName = typeof(TRequest).Name;
                var requestType = typeof(TRequest).FullName;

                _logger.LogWarning(
                    "Long running request detected. Request: {RequestName} ({RequestType}) took {ElapsedMilliseconds}ms. {@Request}",
                    requestName,
                    requestType,
                    elapsedMilliseconds,
                    request);

                Log.Warning(
                    "Long running request detected. Request: {RequestName} ({RequestType}) took {ElapsedMilliseconds}ms. {@Request}",
                    requestName,
                    requestType,
                    elapsedMilliseconds,
                    request);
            }
        }
    }
}