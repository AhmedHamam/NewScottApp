using MediatR;

namespace Base.Application.Security;

public abstract class ActionValidator<TRequest, TResponse> : IActionValidator<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public abstract Task<ActionValidatorResult> Validate(TRequest request, CancellationToken cancellationToken);

    protected ActionValidatorResult NotFound(string? message = default) =>
        new() { Status = ActionValidationStatus.NotFound, Message = message };

    protected ActionValidatorResult Unauthorized(string? message = default) =>
        new() { Status = ActionValidationStatus.Unauthorized, Message = message };

    protected ActionValidatorResult Forbidden(string? message = default) =>
        new() { Status = ActionValidationStatus.Forbidden, Message = message };

    protected ActionValidatorResult Continue() => new() { Status = ActionValidationStatus.Continue };
}

public abstract class ActionValidator<TRequest> : IActionValidator<TRequest, Unit>
    where TRequest : IRequest<Unit>
{
    public abstract Task<ActionValidatorResult> Validate(TRequest request, CancellationToken cancellationToken);

    protected ActionValidatorResult NotFound(string? message = default) =>
        new() { Status = ActionValidationStatus.NotFound, Message = message };

    protected ActionValidatorResult Unauthorized(string? message = default) =>
        new() { Status = ActionValidationStatus.Unauthorized, Message = message };

    protected ActionValidatorResult Forbidden(string? message = default) =>
        new() { Status = ActionValidationStatus.Forbidden, Message = message };

    protected ActionValidatorResult Continue() => new() { Status = ActionValidationStatus.Continue };
}