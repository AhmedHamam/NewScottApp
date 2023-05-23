namespace Base.Domain.CommonInterfaces
{
    public interface ICreatedAuditableEntity
    {
        public string? CreatedBy { get; }
        public DateTime? CreationDate { get; }
        public void MarkAsCreated(string createdBy);
    }
}
