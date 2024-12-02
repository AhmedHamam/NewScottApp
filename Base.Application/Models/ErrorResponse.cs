using System.Text.Json.Serialization;

namespace Base.Application.Models;

/// <summary>
/// Standardized error response model
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Unique identifier for the request
    /// </summary>
    public string TraceId { get; set; }

    /// <summary>
    /// HTTP status code
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Error title/category
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Detailed error message
    /// </summary>
    public string Detail { get; set; }

    /// <summary>
    /// UTC timestamp when the error occurred
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Dictionary of validation errors where key is the property name
    /// and value is an array of error messages
    /// </summary>
    public Dictionary<string, string[]> Errors { get; set; }

    /// <summary>
    /// Additional details shown only in development environment
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DevelopmentErrorDetails DevelopmentDetails { get; set; }
}

/// <summary>
/// Additional error details shown only in development environment
/// </summary>
public class DevelopmentErrorDetails
{
    /// <summary>
    /// Full name of the exception type
    /// </summary>
    public string ExceptionType { get; set; }

    /// <summary>
    /// Exception stack trace
    /// </summary>
    public string StackTrace { get; set; }

    /// <summary>
    /// Inner exception message if present
    /// </summary>
    public string InnerException { get; set; }
}
