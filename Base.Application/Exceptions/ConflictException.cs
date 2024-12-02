using System.Runtime.Serialization;

namespace Base.Application.Exceptions;

/// <summary>
/// Exception thrown when a conflict occurs while processing the request
/// </summary>
[Serializable]
public class ConflictException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the ConflictException class
    /// </summary>
    public ConflictException()
        : base("A conflict occurred while processing the request")
    {
    }

    /// <summary>
    /// Initializes a new instance of the ConflictException class with a specified error message
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public ConflictException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ConflictException class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public ConflictException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ConflictException class with entity details
    /// </summary>
    /// <param name="entityName">Name of the entity that caused the conflict</param>
    /// <param name="conflictReason">The reason for the conflict</param>
    public ConflictException(string entityName, string conflictReason)
        : base($"A conflict occurred with entity '{entityName}': {conflictReason}")
    {
        EntityName = entityName;
        ConflictReason = conflictReason;
    }

    /// <summary>
    /// Initializes a new instance of the ConflictException class with serialized data
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    protected ConflictException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        EntityName = info.GetString(nameof(EntityName));
        ConflictReason = info.GetString(nameof(ConflictReason));
    }

    /// <summary>
    /// Gets the name of the entity that caused the conflict
    /// </summary>
    public string? EntityName { get; }

    /// <summary>
    /// Gets the reason for the conflict
    /// </summary>
    public string? ConflictReason { get; }

    /// <summary>
    /// Sets the SerializationInfo with information about the exception
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(EntityName), EntityName);
        info.AddValue(nameof(ConflictReason), ConflictReason);
    }
}
