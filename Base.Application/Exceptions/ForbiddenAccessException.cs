using System.Runtime.Serialization;

namespace Base.Application.Exceptions;

/// <summary>
/// Exception thrown when access to a resource is forbidden
/// </summary>
[Serializable]
public class ForbiddenAccessException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the ForbiddenAccessException class
    /// </summary>
    public ForbiddenAccessException()
        : base("Access to the requested resource is forbidden")
    {
    }

    /// <summary>
    /// Initializes a new instance of the ForbiddenAccessException class with a specified error message
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public ForbiddenAccessException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ForbiddenAccessException class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public ForbiddenAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ForbiddenAccessException class with a resource name and identifier
    /// </summary>
    /// <param name="resourceName">Name of the resource access was denied to</param>
    /// <param name="resourceId">Identifier of the resource</param>
    public ForbiddenAccessException(string resourceName, object resourceId)
        : base($"Access denied to resource \"{resourceName}\" with identifier ({resourceId})")
    {
        ResourceName = resourceName;
        ResourceId = resourceId?.ToString();
    }

    /// <summary>
    /// Initializes a new instance of the ForbiddenAccessException class with serialized data
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    protected ForbiddenAccessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        ResourceName = info.GetString(nameof(ResourceName));
        ResourceId = info.GetString(nameof(ResourceId));
    }

    /// <summary>
    /// Gets the name of the resource that access was denied to
    /// </summary>
    public string? ResourceName { get; }

    /// <summary>
    /// Gets the identifier of the resource that access was denied to
    /// </summary>
    public string? ResourceId { get; }

    /// <summary>
    /// Sets the SerializationInfo with information about the exception
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(ResourceName), ResourceName);
        info.AddValue(nameof(ResourceId), ResourceId);
    }
}