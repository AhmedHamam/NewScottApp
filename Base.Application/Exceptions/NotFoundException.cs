using System.Runtime.Serialization;

namespace Base.Application.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found
/// </summary>
[Serializable]
public class NotFoundException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the NotFoundException class
    /// </summary>
    public NotFoundException()
        : base("The requested resource was not found")
    {
    }

    /// <summary>
    /// Initializes a new instance of the NotFoundException class with a specified error message
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public NotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the NotFoundException class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the NotFoundException class with an entity name and identifier
    /// </summary>
    /// <param name="entityName">Name of the entity that was not found</param>
    /// <param name="entityId">Identifier used to look for the entity</param>
    public NotFoundException(string entityName, object entityId)
        : base($"Entity \"{entityName}\" with identifier ({entityId}) was not found")
    {
        EntityName = entityName;
        EntityId = entityId?.ToString();
    }

    /// <summary>
    /// Initializes a new instance of the NotFoundException class with serialized data
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    protected NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        EntityName = info.GetString(nameof(EntityName));
        EntityId = info.GetString(nameof(EntityId));
    }

    /// <summary>
    /// Gets the name of the entity that was not found
    /// </summary>
    public string? EntityName { get; }

    /// <summary>
    /// Gets the identifier used to look for the entity
    /// </summary>
    public string? EntityId { get; }

    /// <summary>
    /// Sets the SerializationInfo with information about the exception
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(EntityName), EntityName);
        info.AddValue(nameof(EntityId), EntityId);
    }
}