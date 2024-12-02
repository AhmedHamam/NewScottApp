namespace Base.Application.Security;

/// <summary>
/// Represents the validation status of an action
/// </summary>
public enum ActionValidationStatus
{
    /// <summary>
    /// The user is not authenticated to perform the action
    /// </summary>
    Unauthorized = 401,

    /// <summary>
    /// The user is authenticated but not authorized to perform the action
    /// </summary>
    Forbidden = 403,

    /// <summary>
    /// The requested resource was not found
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// The action validation passed and can continue
    /// </summary>
    Continue = 100,

    /// <summary>
    /// The request is invalid or malformed
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// A conflict occurred while processing the request
    /// </summary>
    Conflict = 409
}