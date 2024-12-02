namespace Base.Application.Security;

/// <summary>
/// Represents the context for authorization checks, tracking successes and failures
/// </summary>
public class AuthorizationContext
{
    private int _numberOfFails;
    private int _numberOfSuccess;
    private readonly List<string> _errors = new();
    private readonly Dictionary<string, object> _metadata = new();

    /// <summary>
    /// Gets the collection of error messages from failed authorization checks
    /// </summary>
    public IReadOnlyCollection<string> Errors => _errors;

    /// <summary>
    /// Gets the number of failed authorization checks
    /// </summary>
    public int FailureCount => _numberOfFails;

    /// <summary>
    /// Gets the number of successful authorization checks
    /// </summary>
    public int SuccessCount => _numberOfSuccess;

    /// <summary>
    /// Gets a value indicating whether any authorization check has failed or no checks have succeeded
    /// </summary>
    /// <returns>True if authorization has failed, false otherwise</returns>
    public bool MarkedAsFailed() => _numberOfFails > 0 || _numberOfSuccess == 0;

    /// <summary>
    /// Gets a value indicating whether all authorization checks have succeeded
    /// </summary>
    public bool IsSuccessful => !MarkedAsFailed();

    /// <summary>
    /// Gets the metadata associated with the authorization context
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata => _metadata;

    /// <summary>
    /// Marks an authorization check as succeeded
    /// </summary>
    public void MarkAsSucceeded()
    {
        _numberOfSuccess++;
    }

    /// <summary>
    /// Marks an authorization check as succeeded with associated metadata
    /// </summary>
    /// <param name="key">The key for the metadata</param>
    /// <param name="value">The metadata value</param>
    public void MarkAsSucceeded(string key, object value)
    {
        MarkAsSucceeded();
        AddMetadata(key, value);
    }

    /// <summary>
    /// Marks an authorization check as failed
    /// </summary>
    /// <param name="error">Optional error message describing the failure</param>
    public void MarkAsFailed(string? error = default)
    {
        _numberOfFails++;
        if (!string.IsNullOrEmpty(error))
        {
            _errors.Add(error);
        }
    }

    /// <summary>
    /// Marks an authorization check as failed with associated metadata
    /// </summary>
    /// <param name="error">Error message describing the failure</param>
    /// <param name="key">The key for the metadata</param>
    /// <param name="value">The metadata value</param>
    public void MarkAsFailed(string error, string key, object value)
    {
        MarkAsFailed(error);
        AddMetadata(key, value);
    }

    /// <summary>
    /// Adds metadata to the authorization context
    /// </summary>
    /// <param name="key">The key for the metadata</param>
    /// <param name="value">The metadata value</param>
    public void AddMetadata(string key, object value)
    {
        _metadata[key] = value;
    }

    /// <summary>
    /// Gets metadata value by key
    /// </summary>
    /// <typeparam name="T">The type of the metadata value</typeparam>
    /// <param name="key">The key of the metadata</param>
    /// <returns>The metadata value if found and of correct type, default(T) otherwise</returns>
    public T? GetMetadata<T>(string key)
    {
        if (_metadata.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default;
    }

    /// <summary>
    /// Clears all errors and resets success/failure counts
    /// </summary>
    public void Reset()
    {
        _numberOfFails = 0;
        _numberOfSuccess = 0;
        _errors.Clear();
        _metadata.Clear();
    }
}