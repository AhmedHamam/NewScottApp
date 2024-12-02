using System.Runtime.Serialization;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Base.Application.Exceptions;

/// <summary>
/// Exception thrown when validation fails for a request
/// </summary>
[Serializable]
public sealed class ValidationException : ApplicationException
{
    /// <summary>
    /// Initializes a new instance of the ValidationException class
    /// </summary>
    /// <param name="errorsDictionary">Dictionary containing validation errors</param>
    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
        : base("One or more validation failures have occurred.")
    {
        ErrorsDictionary = errorsDictionary ?? throw new ArgumentNullException(nameof(errorsDictionary));
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with FluentValidation results
    /// </summary>
    /// <param name="failures">Collection of validation failures</param>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this(failures.GroupBy(
            e => e.PropertyName,
            e => e.ErrorMessage,
            (propertyName, errorMessages) => new
            {
                Key = propertyName,
                Values = errorMessages.Distinct().ToArray()
            })
            .ToDictionary(e => e.Key, e => e.Values))
    {
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException class with serialized data
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    private ValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        var count = info.GetInt32("Count");
        var errors = new Dictionary<string, string[]>(count);
        
        for (var i = 0; i < count; i++)
        {
            var key = info.GetString($"Key_{i}");
            var values = (string[])info.GetValue($"Values_{i}", typeof(string[]));
            if (key != null)
            {
                errors.Add(key, values);
            }
        }

        ErrorsDictionary = errors;
    }

    /// <summary>
    /// Gets the dictionary of validation errors
    /// </summary>
    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }

    /// <summary>
    /// Sets the SerializationInfo with information about the exception
    /// </summary>
    /// <param name="info">The SerializationInfo that holds the serialized object data</param>
    /// <param name="context">The StreamingContext that contains contextual information about the source or destination</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);

        info.AddValue("Count", ErrorsDictionary.Count);
        var i = 0;
        foreach (var kvp in ErrorsDictionary)
        {
            info.AddValue($"Key_{i}", kvp.Key);
            info.AddValue($"Values_{i}", kvp.Value);
            i++;
        }
    }
}