namespace Base.Application.Security;

public class AuthorizationContext
{
    private int _numberOfFails;
    private int _numberOfSuccess;
    private readonly List<string> _errors = new();

    public IReadOnlyCollection<string> Errors => _errors;
    public bool MarkedAsFailed() => _numberOfFails > 0 || _numberOfSuccess == 0;

    public void MarkAsSucceeded()
    {
        _numberOfSuccess++;
    }

    public void MarkAsFailed(string? error = default)
    {
        _numberOfFails++;
        if (!string.IsNullOrEmpty(error))
            _errors.Add(error);
    }
}