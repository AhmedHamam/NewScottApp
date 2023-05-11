namespace Base.Domain.CommonInterfaces
{
    public interface ICreatedAuditableEntity<T>
    {
        public T CreatedBy { get; }
        public DateTimeOffset CreatedOn { get; }
        public void MarkAsCreated(T createdBy);
    }
}
