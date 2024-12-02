namespace Base.Application.Models;

/// <summary>
/// Represents the result of an operation with success status and optional error messages
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the Result class
    /// </summary>
    /// <param name="succeeded">Indicates whether the operation succeeded</param>
    /// <param name="errors">Collection of error messages if the operation failed</param>
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    /// <summary>
    /// Gets or sets a value indicating whether the operation succeeded
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Gets or sets the array of error messages
    /// </summary>
    public string[] Errors { get; set; }

    /// <summary>
    /// Creates a success result
    /// </summary>
    /// <returns>A new Result instance indicating success</returns>
    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    /// <summary>
    /// Creates a failure result with the specified error messages
    /// </summary>
    /// <param name="errors">Collection of error messages</param>
    /// <returns>A new Result instance indicating failure</returns>
    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }

    /// <summary>
    /// Creates a failure result with a single error message
    /// </summary>
    /// <param name="error">The error message</param>
    /// <returns>A new Result instance indicating failure</returns>
    public static Result Failure(string error)
    {
        return new Result(false, new[] { error });
    }
}

/// <summary>
/// Represents the result of an operation that returns data, with success status and optional error messages
/// </summary>
/// <typeparam name="T">The type of the data returned by the operation</typeparam>
public class Result<T> : Result
{
    /// <summary>
    /// Initializes a new instance of the Result class
    /// </summary>
    /// <param name="succeeded">Indicates whether the operation succeeded</param>
    /// <param name="data">The data returned by the operation</param>
    /// <param name="errors">Collection of error messages if the operation failed</param>
    internal Result(bool succeeded, T? data, IEnumerable<string> errors)
        : base(succeeded, errors)
    {
        Data = data;
    }

    /// <summary>
    /// Gets the data returned by the operation
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// Creates a success result with the specified data
    /// </summary>
    /// <param name="data">The data to return</param>
    /// <returns>A new Result instance indicating success with data</returns>
    public static Result<T> Success(T data)
    {
        return new Result<T>(true, data, Array.Empty<string>());
    }

    /// <summary>
    /// Creates a failure result with the specified error messages
    /// </summary>
    /// <param name="errors">Collection of error messages</param>
    /// <returns>A new Result instance indicating failure</returns>
    public new static Result<T> Failure(IEnumerable<string> errors)
    {
        return new Result<T>(false, default, errors);
    }

    /// <summary>
    /// Creates a failure result with a single error message
    /// </summary>
    /// <param name="error">The error message</param>
    /// <returns>A new Result instance indicating failure</returns>
    public new static Result<T> Failure(string error)
    {
        return new Result<T>(false, default, new[] { error });
    }
}