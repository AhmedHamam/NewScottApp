using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Base.Application.Behaviours;

/// <summary>
/// MediatR pipeline behavior that performs validation using FluentValidation
/// </summary>
/// <typeparam name="TRequest">The type of request being handled</typeparam>
/// <typeparam name="TResponse">The type of response from the handler</typeparam>
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IHttpContextAccessor _accessor;

    /// <summary>
    /// Initializes a new instance of the ValidationBehaviour class
    /// </summary>
    /// <param name="validators">Collection of validators for the request type</param>
    /// <param name="accessor">HTTP context accessor for handling validation responses</param>
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, IHttpContextAccessor accessor)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }

    /// <summary>
    /// Handles the request by performing validation before passing to the next handler
    /// </summary>
    /// <param name="request">The incoming request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="next">Delegate to the next handler in the pipeline</param>
    /// <returns>The response from the next handler if validation passes</returns>
    /// <remarks>
    /// The validation process:
    /// 1. Checks if any validators exist for the request type
    /// 2. Executes all validators and collects results
    /// 3. Groups validation errors by property name
    /// 4. Returns appropriate response based on validation results
    /// </remarks>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        // Skip validation if no validators are registered
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var errorsDictionary = validationResults
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);

        // Continue to next handler if validation passes
        if (!errorsDictionary.Any())
            return await next();

        // Handle validation failure
        await _accessor.SendBadRequestAndAbort("Validation errors", errorsDictionary);
        
        // Return appropriate default value based on response type
        return CreateDefaultResponse();
    }

    /// <summary>
    /// Creates a default response based on the TResponse type
    /// </summary>
    /// <returns>A default instance of TResponse</returns>
    private static TResponse CreateDefaultResponse()
    {
        if (typeof(TResponse) == typeof(string))
            return (TResponse)(object)"";
        
        if (typeof(TResponse) == typeof(Guid))
            return (TResponse)(object)Guid.Empty;
        
        return Activator.CreateInstance<TResponse>();
    }
}