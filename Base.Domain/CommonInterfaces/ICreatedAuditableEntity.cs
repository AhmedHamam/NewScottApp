namespace Base.Domain.CommonInterfaces
{
    public interface ICreatedAuditableEntity
    {
        public string CreatedBy { get; }
        public DateTimeOffset CreationDate { get; }
        public void MarkAsCreated(string createdBy);
    }
}
