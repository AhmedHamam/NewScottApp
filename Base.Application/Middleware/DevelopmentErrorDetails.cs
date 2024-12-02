namespace Base.Application.Middleware;

/// <summary>
/// Additional error details shown only in development environment
/// </summary>
public class DevelopmentErrorDetails
{
    public string? ExceptionType { get; set; }
    public string? StackTrace { get; set; }
    public string? InnerException { get; set; }
}
