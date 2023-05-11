namespace Base.Application.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException()
    {
    }

    public ForbiddenAccessException(string message) : base(message)
    {
    }

    public ForbiddenAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ForbiddenAccessException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}