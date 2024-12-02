using MediatR;

namespace Base.Application.Security;

/// <summary>
/// Defines a validator for requests that return a response
/// </summary>
/// <typeparam name="TRequest">The type of request being validated</typeparam>
/// <typeparam name="TResponse">The type of response expected from the request</typeparam>
public interface IActionValidator<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Validates the request
    /// </summary>
    /// <param name="request">The request to validate</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>A task that represents the asynchronous validation operation. The task result contains the validation result.</returns>
    Task<ActionValidatorResult> Validate(TRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Validates the request synchronously
    /// </summary>
    /// <param name="request">The request to validate</param>
    /// <returns>The validation result</returns>
    /// <remarks>
    /// This method should only be used when synchronous validation is absolutely necessary.
    /// Prefer using the asynchronous <see cref="Validate"/> method whenever possible.
    /// </remarks>
    ActionValidatorResult ValidateSync(TRequest request) =>
        Validate(request, CancellationToken.None).GetAwaiter().GetResult();
}

/// <summary>
/// Defines a validator for requests that do not return a response (Unit)
/// </summary>
/// <typeparam name="TRequest">The type of request being validated</typeparam>
public interface IActionValidator<in TRequest> : IActionValidator<TRequest, Unit>
    where TRequest : IRequest<Unit>
{
}