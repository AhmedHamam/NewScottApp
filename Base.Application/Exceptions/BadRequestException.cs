using System.Runtime.Serialization;

namespace Base.Application.Exceptions;

/// <summary>
/// Exception thrown when the request is malformed or invalid
/// </summary>
[Serializable]
public class BadRequestException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the BadRequestException class
    /// </summary>
    public BadRequestException()
        : base("The request was invalid or malformed")
    {
    }

    /// <summary>
    /// Initializes a new instance of the BadRequestException class with a specified error message
    /// </summary>
    /// <param name="message">The message that describes the error</param>
    public BadRequestException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the BadRequestException class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the BadRequestException class with a property name and reason
    /// </summary>
    /// <param name="propertyName">Name of the property that caused the error</param>
    /// <param name="reason">The reason why the request was invalid</param>
    public BadRequestException(string propertyName, string reason)
        : base($"Property '{propertyName}' is invalid: {reason}")
    {
        PropertyName = propertyName;
        Reason = reason;
    }

    /// <summary>
    /// Initializes a new instance of the BadRequestException class with serialized data
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    protected BadRequestException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        PropertyName = info.GetString(nameof(PropertyName));
        Reason = info.GetString(nameof(Reason));
    }

    /// <summary>
    /// Gets the name of the property that caused the error
    /// </summary>
    public string? PropertyName { get; }

    /// <summary>
    /// Gets the reason why the request was invalid
    /// </summary>
    public string? Reason { get; }

    /// <summary>
    /// Sets the SerializationInfo with information about the exception
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(PropertyName), PropertyName);
        info.AddValue(nameof(Reason), Reason);
    }
}
