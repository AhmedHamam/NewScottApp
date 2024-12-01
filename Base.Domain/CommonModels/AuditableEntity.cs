using Base.Domain.CommonInterfaces;
using Base.Domain.ValueObjects;

namespace Base.Domain.CommonModels
{
    public abstract class AuditableEntity : BaseEntity,
        ICreatedAuditableEntity, IModifiedAuditableEntity
    {
        public UserId CreatedBy { get; private set; }
        public DateTime? CreationDate { get; private set; }

        public UserId? UpdatedBy { get; private set; }
        public DateTime? UpdatedDate { get; private set; }

        public bool? IsUpdated { get; private set; }

        public void MarkAsCreated(string createdBy)
        {
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("CreatedBy cannot be empty", nameof(createdBy));

            if (CreationDate.HasValue)
                throw new InvalidOperationException("Entity is already marked as created");

            CreatedBy = new UserId(createdBy);
            CreationDate = DateTime.UtcNow;
        }

        public void MarkAsUpdated(string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("UpdatedBy cannot be empty", nameof(updatedBy));

            if (!CreationDate.HasValue)
                throw new InvalidOperationException("Entity must be created before it can be updated");

            UpdatedBy = new UserId(updatedBy);
            UpdatedDate = DateTime.UtcNow;
            IsUpdated = true;
        }
    }
}
