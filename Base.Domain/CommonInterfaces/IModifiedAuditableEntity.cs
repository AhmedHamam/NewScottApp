namespace Base.Domain.CommonInterfaces
{
    public interface IModifiedAuditableEntity<T>
    {
        public T ModifiedBy { get; }
        public DateTimeOffset ModifiedOn { get; }
        public void MarkAsModified(T modifiedBy);
    }
}
