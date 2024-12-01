namespace Base.Domain.Services
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
