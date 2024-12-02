namespace Base.Application.Security;

/// <summary>
/// Represents the result of an action validation
/// </summary>
public class ActionValidatorResult
{
    /// <summary>
    /// Gets or sets the validation status
    /// </summary>
    public ActionValidationStatus Status { get; init; }

    /// <summary>
    /// Gets or sets an optional message providing more details about the validation result
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Gets a value indicating whether the validation passed and the action can continue
    /// </summary>
    public bool CanContinue => Status == ActionValidationStatus.Continue;

    /// <summary>
    /// Gets a value indicating whether the validation resulted in an error
    /// </summary>
    public bool HasError => Status != ActionValidationStatus.Continue;

    /// <summary>
    /// Creates a new instance of ActionValidatorResult with Continue status
    /// </summary>
    /// <returns>A success validation result</returns>
    public static ActionValidatorResult Success() => new() { Status = ActionValidationStatus.Continue };

    /// <summary>
    /// Creates a new instance of ActionValidatorResult with NotFound status
    /// </summary>
    /// <param name="message">Optional message describing why the resource was not found</param>
    /// <returns>A not found validation result</returns>
    public static ActionValidatorResult NotFound(string? message = null) => 
        new() { Status = ActionValidationStatus.NotFound, Message = message };

    /// <summary>
    /// Creates a new instance of ActionValidatorResult with Unauthorized status
    /// </summary>
    /// <param name="message">Optional message describing why the request was unauthorized</param>
    /// <returns>An unauthorized validation result</returns>
    public static ActionValidatorResult Unauthorized(string? message = null) => 
        new() { Status = ActionValidationStatus.Unauthorized, Message = message };

    /// <summary>
    /// Creates a new instance of ActionValidatorResult with Forbidden status
    /// </summary>
    /// <param name="message">Optional message describing why the request was forbidden</param>
    /// <returns>A forbidden validation result</returns>
    public static ActionValidatorResult Forbidden(string? message = null) => 
        new() { Status = ActionValidationStatus.Forbidden, Message = message };

    /// <summary>
    /// Creates a new instance of ActionValidatorResult with BadRequest status
    /// </summary>
    /// <param name="message">Optional message describing why the request was invalid</param>
    /// <returns>A bad request validation result</returns>
    public static ActionValidatorResult BadRequest(string? message = null) => 
        new() { Status = ActionValidationStatus.BadRequest, Message = message };

    /// <summary>
    /// Creates a new instance of ActionValidatorResult with Conflict status
    /// </summary>
    /// <param name="message">Optional message describing the conflict</param>
    /// <returns>A conflict validation result</returns>
    public static ActionValidatorResult Conflict(string? message = null) => 
        new() { Status = ActionValidationStatus.Conflict, Message = message };
}