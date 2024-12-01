using Base.Domain.ValueObjects;

namespace Base.Domain.CommonInterfaces
{
    public interface ICreatedAuditableEntity
    {
        public UserId CreatedBy { get; }
        public DateTime? CreationDate { get; }
        public void MarkAsCreated(string createdBy);
    }
}
