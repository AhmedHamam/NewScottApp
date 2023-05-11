namespace Base.Application.Security;

public class NotFoundContext
{
    private bool _notFound;
    private readonly List<string> _errors = new();

    public IReadOnlyCollection<string> Errors => _errors;
    public bool MarkedAsNotFound() => _notFound;

    public void MarkAsNotFound(string? error = default)
    {
        _notFound = true;
        if (!string.IsNullOrEmpty(error))
            _errors.Add(error);
    }
}