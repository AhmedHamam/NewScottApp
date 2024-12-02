using MediatR;

namespace Base.Application.Security;

/// <summary>
/// Base class for action validators that validate requests with a response
/// </summary>
/// <typeparam name="TRequest">The type of request being validated</typeparam>
/// <typeparam name="TResponse">The type of response expected from the request</typeparam>
public abstract class ActionValidator<TRequest, TResponse> : IActionValidator<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Validates the request
    /// </summary>
    /// <param name="request">The request to validate</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>The validation result</returns>
    public abstract Task<ActionValidatorResult> Validate(TRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a success result indicating the validation passed
    /// </summary>
    /// <returns>An ActionValidatorResult with Continue status</returns>
    protected ActionValidatorResult Success() => ActionValidatorResult.Success();

    /// <summary>
    /// Creates a NotFound result with an optional message
    /// </summary>
    /// <param name="message">Optional message describing why the resource was not found</param>
    /// <returns>An ActionValidatorResult with NotFound status</returns>
    protected ActionValidatorResult NotFound(string? message = default) => ActionValidatorResult.NotFound(message);

    /// <summary>
    /// Creates an Unauthorized result with an optional message
    /// </summary>
    /// <param name="message">Optional message describing why the request was unauthorized</param>
    /// <returns>An ActionValidatorResult with Unauthorized status</returns>
    protected ActionValidatorResult Unauthorized(string? message = default) => ActionValidatorResult.Unauthorized(message);

    /// <summary>
    /// Creates a Forbidden result with an optional message
    /// </summary>
    /// <param name="message">Optional message describing why the request was forbidden</param>
    /// <returns>An ActionValidatorResult with Forbidden status</returns>
    protected ActionValidatorResult Forbidden(string? message = default) => ActionValidatorResult.Forbidden(message);

    /// <summary>
    /// Creates a BadRequest result with an optional message
    /// </summary>
    /// <param name="message">Optional message describing why the request was invalid</param>
    /// <returns>An ActionValidatorResult with BadRequest status</returns>
    protected ActionValidatorResult BadRequest(string? message = default) => ActionValidatorResult.BadRequest(message);

    /// <summary>
    /// Creates a Conflict result with an optional message
    /// </summary>
    /// <param name="message">Optional message describing the conflict</param>
    /// <returns>An ActionValidatorResult with Conflict status</returns>
    protected ActionValidatorResult Conflict(string? message = default) => ActionValidatorResult.Conflict(message);
}

/// <summary>
/// Base class for action validators that validate requests without a response (Unit)
/// </summary>
/// <typeparam name="TRequest">The type of request being validated</typeparam>
public abstract class ActionValidator<TRequest> : ActionValidator<TRequest, Unit>
    where TRequest : IRequest<Unit>
{
}