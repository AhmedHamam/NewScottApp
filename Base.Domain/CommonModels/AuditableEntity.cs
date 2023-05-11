using Base.Domain.CommonInterfaces;

namespace Base.Domain.CommonModels
{
    public class AuditableEntity<T> : BaseEntity<T>,
        ICreatedAuditableEntity<T>, IModifiedAuditableEntity<T>
    {
        public DateTimeOffset CreatedOn { get; private set; }

        public T CreatedBy { get; private set; }
        public DateTimeOffset ModifiedOn { get; private set; }
        public T ModifiedBy { get; private set; }

        public void MarkAsCreated(T createdBy)
        {
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
        }

        public void MarkAsModified(T modifiedBy)
        {
            ModifiedBy = modifiedBy;
            ModifiedOn = DateTime.UtcNow;
        }
    }
}
