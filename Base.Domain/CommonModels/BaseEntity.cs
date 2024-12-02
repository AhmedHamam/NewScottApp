using Base.Domain.CommonInterfaces;
using Base.Domain.DomainEvents;
using Base.Domain.ValueObjects;
using System.Collections.Generic;

namespace Base.Domain.CommonModels
{
    /// <summary>
    /// Base class for all domain entities, providing soft delete functionality and domain event handling
    /// </summary>
    public abstract class BaseEntity : ISoftDelete
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        
        /// <summary>
        /// Gets a value indicating whether this entity is marked as deleted
        /// </summary>
        public bool IsDeleted { get; private set; } = false;

        /// <summary>
        /// Gets the date when this entity was deleted
        /// </summary>
        public DateTime? DeletedDate { get; private set; }

        /// <summary>
        /// Gets the user who deleted this entity
        /// </summary>
        public UserId DeletedBy { get; private set; }

        /// <summary>
        /// Gets the collection of domain events raised by this entity
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Marks the entity as deleted
        /// </summary>
        /// <param name="deletedBy">The user who is deleting the entity</param>
        /// <exception cref="ArgumentException">Thrown when deletedBy is null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when entity is already deleted</exception>
        public void MarkAsDeleted(string deletedBy)
        {
            if (string.IsNullOrEmpty(deletedBy))
            {
                throw new ArgumentException("DeletedBy cannot be empty", nameof(deletedBy));
            }
        
            if (IsDeleted)
            {
                throw new InvalidOperationException("Entity is already deleted");
            }
        
            IsDeleted = true;
            DeletedDate = DateTime.UtcNow;
            DeletedBy = new UserId(deletedBy);
        
            _domainEvents.Add(new EntityDeletedEvent(DeletedBy));
            _domainEvents.Add(new EntityDeletedEvent(new UserId(deletedBy)));
        }

        /// <summary>
        /// Marks the entity as not deleted, reversing a soft delete
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when entity is not deleted</exception>
        public void MarkAsNotDeleted()
        {
            if (!IsDeleted)
                throw new InvalidOperationException("Entity is not deleted");

            IsDeleted = false;
            DeletedDate = null;
            DeletedBy = null;
        }

        /// <summary>
        /// Adds a domain event to this entity's collection of events
        /// </summary>
        /// <param name="domainEvent">The domain event to add</param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clears all domain events from this entity
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
