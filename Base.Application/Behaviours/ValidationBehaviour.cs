using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Base.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IHttpContextAccessor _accessor;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, IHttpContextAccessor accessor)
    {
        _validators = validators;
        _accessor = accessor;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = new List<ValidationResult>();

        foreach (var validator in _validators)
            validationResults.Add(await validator.ValidateAsync(context, cancellationToken));

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

        if (!errorsDictionary.Any())
            return await next();

        await _accessor.SendBadRequestAndAbort("Validation errors", errorsDictionary);
        if (typeof(TResponse) == typeof(string))
            return (TResponse)Activator.CreateInstance(typeof(string), "".ToCharArray())!;
        if (typeof(TResponse) == typeof(Guid))
            return (TResponse)Activator.CreateInstance(typeof(Guid), Guid.Empty)!;
        return (TResponse)Activator.CreateInstance(typeof(TResponse))!;
    }
}