using Base.Domain.ValueObjects;

namespace Base.Domain.DomainEvents
{
    /// <summary>
    /// Represents a domain event that is raised when an entity is marked as deleted
    /// </summary>
    /// <remarks>
    /// This event is typically raised by entities implementing the ISoftDelete interface
    /// when they are marked as deleted. It carries information about who performed the
    /// deletion and when it occurred.
    /// </remarks>
    public class EntityDeletedEvent : IDomainEvent
    {
        /// <summary>
        /// Gets the identifier of the user who deleted the entity
        /// </summary>
        public UserId DeletedBy { get; }

        /// <summary>
        /// Gets the date and time when this domain event occurred
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityDeletedEvent"/> class
        /// </summary>
        /// <param name="deletedBy">The identifier of the user who deleted the entity</param>
        /// <exception cref="ArgumentNullException">Thrown when deletedBy is null</exception>
        public EntityDeletedEvent(UserId deletedBy)
        {
            DeletedBy = deletedBy ?? throw new ArgumentNullException(nameof(deletedBy));
            OccurredOn = DateTime.UtcNow;
        }
    }
}
