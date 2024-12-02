namespace Base.Application.Middleware;

/// <summary>
/// Standardized error response model
/// </summary>
public class ErrorResponse
{
    public string TraceId { get; set; }
    public int StatusCode { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }
    public DateTime Timestamp { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
    public DevelopmentErrorDetails DevelopmentDetails { get; set; }
}
