namespace Base.Domain.CommonInterfaces
{
    public interface IHasStatus<TStatus> where TStatus : struct
    {
        TStatus Status { get; }
        void UpdateStatus(TStatus newStatus, string updatedBy);
    }
}
