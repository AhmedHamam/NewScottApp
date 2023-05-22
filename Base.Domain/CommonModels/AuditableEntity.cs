using Base.Domain.CommonInterfaces;

namespace Base.Domain.CommonModels
{
    public class AuditableEntity : BaseEntity,
        ICreatedAuditableEntity, IModifiedAuditableEntity
    {
        public string CreatedBy { get; private set; }
        public DateTimeOffset CreationDate { get; private set; }

        public string UpdatedBy { get; private set; }
        public DateTimeOffset UpdatedDate { get; private set; }

        public bool IsUpdated { get; private set; }

        public void MarkAsCreated(string createdBy)
        {
            CreatedBy = createdBy;
            CreationDate = DateTime.UtcNow;
        }

        public void MarkAsUpdated(string updatedBy)
        {
            UpdatedBy = updatedBy;
            UpdatedDate = DateTime.UtcNow;
            IsUpdated = true;
        }
    }
}
