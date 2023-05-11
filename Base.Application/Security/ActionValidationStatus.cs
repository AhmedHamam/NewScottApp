namespace Base.Application.Security;

public enum ActionValidationStatus
{
    Unauthorized = 1,
    Forbidden = 2,
    NotFound = 3,
    Continue = 5,
}