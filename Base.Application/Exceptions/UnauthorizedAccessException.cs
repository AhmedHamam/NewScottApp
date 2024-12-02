using System.Runtime.Serialization;

namespace Base.Application.Exceptions;

/// <summary>
/// Exception thrown when the user is not authenticated to access a resource
/// </summary>
[Serializable]
public class UnauthorizedAccessException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the UnauthorizedAccessException class
    /// </summary>
    public UnauthorizedAccessException()
        : base("Authentication is required to access this resource")
    {
    }

    /// <summary>
    /// Initializes a new instance of the UnauthorizedAccessException class with a specified error message
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public UnauthorizedAccessException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the UnauthorizedAccessException class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public UnauthorizedAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the UnauthorizedAccessException class with resource details
    /// </summary>
    /// <param name="resourceName">Name of the resource that requires authentication</param>
    /// <param name="requiredPermission">The permission required to access the resource</param>
    public UnauthorizedAccessException(string resourceName, string requiredPermission)
        : base($"Authentication required to access '{resourceName}'. Required permission: {requiredPermission}")
    {
        ResourceName = resourceName;
        RequiredPermission = requiredPermission;
    }

    /// <summary>
    /// Initializes a new instance of the UnauthorizedAccessException class with serialized data
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    protected UnauthorizedAccessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ResourceName = info.GetString(nameof(ResourceName));
        RequiredPermission = info.GetString(nameof(RequiredPermission));
    }

    /// <summary>
    /// Gets the name of the resource that requires authentication
    /// </summary>
    public string? ResourceName { get; }

    /// <summary>
    /// Gets the permission required to access the resource
    /// </summary>
    public string? RequiredPermission { get; }

    /// <summary>
    /// Sets the SerializationInfo with information about the exception
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ResourceName), ResourceName);
        info.AddValue(nameof(RequiredPermission), RequiredPermission);
    }
}
