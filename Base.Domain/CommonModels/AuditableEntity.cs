using Base.Domain.CommonInterfaces;
using Base.Domain.ValueObjects;

namespace Base.Domain.CommonModels
{
    /// <summary>
    /// Base class for entities that require full audit trail capabilities
    /// </summary>
    /// <remarks>
    /// This class extends BaseEntity to provide comprehensive auditing functionality,
    /// tracking both creation and modification details. It implements both ICreatedAuditableEntity
    /// and IModifiedAuditableEntity interfaces.
    /// </remarks>
    public abstract class AuditableEntity : BaseEntity,
        ICreatedAuditableEntity, IModifiedAuditableEntity
    {
        /// <summary>
        /// Gets the identifier of the user who created this entity
        /// </summary>
        public UserId CreatedBy { get; private set; }

        /// <summary>
        /// Gets the date and time when this entity was created
        /// </summary>
        public DateTime? CreationDate { get; private set; }

        /// <summary>
        /// Gets the identifier of the user who last updated this entity
        /// </summary>
        public UserId? UpdatedBy { get; private set; }

        /// <summary>
        /// Gets the date and time when this entity was last updated
        /// </summary>
        public DateTime? UpdatedDate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this entity has been updated
        /// </summary>
        public bool? IsUpdated { get; private set; }

        /// <summary>
        /// Marks the entity as created by the specified user
        /// </summary>
        /// <param name="createdBy">The identifier of the user creating the entity</param>
        /// <exception cref="ArgumentException">Thrown when createdBy is null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when the entity is already marked as created</exception>
        public void MarkAsCreated(string createdBy)
        {
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("CreatedBy cannot be empty", nameof(createdBy));

            if (CreationDate.HasValue)
                throw new InvalidOperationException("Entity is already marked as created");

            CreatedBy = new UserId(createdBy);
            CreationDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the entity as updated by the specified user
        /// </summary>
        /// <param name="updatedBy">The identifier of the user updating the entity</param>
        /// <exception cref="ArgumentException">Thrown when updatedBy is null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when the entity hasn't been created yet</exception>
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
