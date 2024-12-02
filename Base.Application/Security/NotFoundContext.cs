namespace Base.Application.Security;

/// <summary>
/// Represents a context for tracking not found states and associated error messages
/// </summary>
public class NotFoundContext
{
    private bool _notFound;
    private readonly List<string> _errors = new();
    private readonly Dictionary<string, object> _metadata = new();

    /// <summary>
    /// Gets the collection of error messages associated with not found states
    /// </summary>
    public IReadOnlyCollection<string> Errors => _errors;

    /// <summary>
    /// Gets the metadata associated with the not found context
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata => _metadata;

    /// <summary>
    /// Gets a value indicating whether any resource has been marked as not found
    /// </summary>
    /// <returns>True if any resource has been marked as not found, false otherwise</returns>
    public bool MarkedAsNotFound() => _notFound;

    /// <summary>
    /// Marks a resource as not found with an optional error message
    /// </summary>
    /// <param name="error">Optional error message describing what was not found</param>
    public void MarkAsNotFound(string? error = default)
    {
        _notFound = true;
        if (!string.IsNullOrEmpty(error))
        {
            _errors.Add(error);
        }
    }

    /// <summary>
    /// Marks a resource as not found with an error message and associated metadata
    /// </summary>
    /// <param name="error">Error message describing what was not found</param>
    /// <param name="key">The key for the metadata</param>
    /// <param name="value">The metadata value</param>
    public void MarkAsNotFound(string error, string key, object value)
    {
        MarkAsNotFound(error);
        AddMetadata(key, value);
    }

    /// <summary>
    /// Adds metadata to the not found context
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
    /// Clears all errors, metadata, and resets the not found state
    /// </summary>
    public void Reset()
    {
        _notFound = false;
        _errors.Clear();
        _metadata.Clear();
    }
}