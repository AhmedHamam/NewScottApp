namespace Base.Domain.CommonInterfaces
{
    public interface ITrackable
    {
        int Version { get; }
        string ConcurrencyStamp { get; }
        void IncrementVersion();
    }
}
