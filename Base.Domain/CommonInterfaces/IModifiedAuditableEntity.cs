using Base.Domain.ValueObjects;

namespace Base.Domain.CommonInterfaces
{
    /// <summary>
    /// Defines an interface for entities that track modification audit information
    /// </summary>
    /// <remarks>
    /// This interface is used to maintain audit trails for when and by whom an entity was last modified.
    /// It's typically used in conjunction with AuditableEntity for full audit capabilities.
    /// </remarks>
    public interface IModifiedAuditableEntity
    {
        /// <summary>
        /// Gets the identifier of the user who last updated this entity
        /// </summary>
        UserId? UpdatedBy { get; }

        /// <summary>
        /// Gets the date and time when this entity was last updated
        /// </summary>
        DateTime? UpdatedDate { get; }

        /// <summary>
        /// Gets a value indicating whether this entity has been updated
        /// </summary>
        bool? IsUpdated { get; }

        /// <summary>
        /// Marks the entity as updated by the specified user
        /// </summary>
        /// <param name="updatedBy">The identifier of the user updating the entity</param>
        /// <exception cref="ArgumentException">Thrown when updatedBy is null or empty</exception>
        void MarkAsUpdated(string updatedBy);
    }
}
