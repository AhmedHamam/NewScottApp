using Base.Domain.ValueObjects;

namespace Base.Domain.CommonInterfaces
{
    /// <summary>
    /// Defines an interface for entities that support soft deletion
    /// </summary>
    /// <remarks>
    /// Soft deletion allows entities to be marked as deleted without physically removing them from the database.
    /// This is useful for maintaining data history, audit trails, and potential data recovery.
    /// </remarks>
    public interface ISoftDelete
    {
        /// <summary>
        /// Gets a value indicating whether this entity has been marked as deleted
        /// </summary>
        bool IsDeleted { get; }

        /// <summary>
        /// Gets the date and time when this entity was deleted
        /// </summary>
        DateTime? DeletedDate { get; }

        /// <summary>
        /// Gets the identifier of the user who deleted this entity
        /// </summary>
        UserId DeletedBy { get; }

        /// <summary>
        /// Marks the entity as deleted by the specified user
        /// </summary>
        /// <param name="deletedBy">The identifier of the user deleting the entity</param>
        /// <exception cref="ArgumentException">Thrown when deletedBy is null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when the entity is already deleted</exception>
        void MarkAsDeleted(string deletedBy);

        /// <summary>
        /// Marks the entity as not deleted, effectively restoring it
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the entity is not deleted</exception>
        void MarkAsNotDeleted();
    }
}
