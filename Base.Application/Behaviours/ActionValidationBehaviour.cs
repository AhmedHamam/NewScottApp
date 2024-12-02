using Base.Application.Security;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Base.Application.Behaviours;

/// <summary>
/// MediatR pipeline behavior that performs action-specific validation and authorization checks
/// </summary>
/// <typeparam name="TRequest">The type of request being handled</typeparam>
/// <typeparam name="TResponse">The type of response from the handler</typeparam>
/// <remarks>
/// This behavior executes custom action validators that can perform complex validation
/// and authorization checks. It supports multiple validation scenarios including:
/// - Authorization checks
/// - Resource access validation
/// - Custom business rule validation
/// </remarks>
public class ActionValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IActionValidator<TRequest, TResponse>> _actionValidators;
    private readonly IHttpContextAccessor _accessor;

    /// <summary>
    /// Initializes a new instance of the ActionValidationBehaviour class
    /// </summary>
    /// <param name="actionValidators">Collection of action validators for the request type</param>
    /// <param name="accessor">HTTP context accessor for handling validation responses</param>
    public ActionValidationBehaviour(
        IEnumerable<IActionValidator<TRequest, TResponse>> actionValidators,
        IHttpContextAccessor accessor)
    {
        _actionValidators = actionValidators ?? throw new ArgumentNullException(nameof(actionValidators));
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }

    /// <summary>
    /// Handles the request by executing all registered action validators
    /// </summary>
    /// <param name="request">The incoming request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="next">Delegate to the next handler in the pipeline</param>
    /// <returns>The response from the next handler if validation passes</returns>
    /// <remarks>
    /// The validation process:
    /// 1. Executes each action validator in sequence
    /// 2. Handles different validation statuses (Unauthorized, Forbidden, NotFound)
    /// 3. Aborts the pipeline with appropriate HTTP response if validation fails
    /// 4. Continues to next handler if all validations pass
    /// </remarks>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (!_actionValidators.Any())
            return await next();

        foreach (var actionValidator in _actionValidators)
        {
            var result = await actionValidator.Validate(request, cancellationToken);
            
            if (await HandleValidationResult(result))
                return CreateDefaultResponse();
        }

        return await next();
    }

    /// <summary>
    /// Handles the validation result and sends appropriate HTTP response if validation fails
    /// </summary>
    /// <param name="result">The validation result to handle</param>
    /// <returns>True if the pipeline should be aborted, false to continue</returns>
    private async Task<bool> HandleValidationResult(ActionValidationResult result)
    {
        switch (result.Status)
        {
            case ActionValidationStatus.Unauthorized:
                await _accessor.SendUnauthorizedAndAbort(result.Message ?? "Unauthorized");
                return true;

            case ActionValidationStatus.Forbidden:
                await _accessor.SendForbiddenAndAbort(result.Message ?? "Forbidden");
                return true;

            case ActionValidationStatus.NotFound:
                await _accessor.SendNotFoundAndAbort(result.Message ?? "Not Found");
                return true;

            case ActionValidationStatus.Continue:
                return false;

            default:
                throw new ArgumentException($"Unsupported validation status: {result.Status}", nameof(result));
        }
    }

    /// <summary>
    /// Creates a default response for when validation fails
    /// </summary>
    /// <returns>A default instance of TResponse</returns>
    private static TResponse CreateDefaultResponse()
    {
        return Activator.CreateInstance<TResponse>();
    }
}