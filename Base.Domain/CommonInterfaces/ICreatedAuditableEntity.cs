using Base.Domain.ValueObjects;

namespace Base.Domain.CommonInterfaces
{
    /// <summary>
    /// Defines an interface for entities that track creation audit information
    /// </summary>
    /// <remarks>
    /// This interface is used to maintain audit trails for when and by whom an entity was created.
    /// It's typically used in conjunction with AuditableEntity for full audit capabilities.
    /// </remarks>
    public interface ICreatedAuditableEntity
    {
        /// <summary>
        /// Gets the identifier of the user who created this entity
        /// </summary>
        UserId CreatedBy { get; }

        /// <summary>
        /// Gets the date and time when this entity was created
        /// </summary>
        DateTime? CreationDate { get; }

        /// <summary>
        /// Marks the entity as created by the specified user
        /// </summary>
        /// <param name="createdBy">The identifier of the user creating the entity</param>
        /// <exception cref="ArgumentException">Thrown when createdBy is null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when the entity is already marked as created</exception>
        void MarkAsCreated(string createdBy);
    }
}
