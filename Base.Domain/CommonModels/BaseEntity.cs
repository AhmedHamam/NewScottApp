using Base.Domain.CommonInterfaces;
using Base.Domain.DomainEvents;
using Base.Domain.ValueObjects;
using System.Collections.Generic;

namespace Base.Domain.CommonModels
{
    public abstract class BaseEntity : ISoftDelete
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedDate { get; private set; }
        public UserId DeletedBy { get; private set; }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void MarkAsDeleted(string deletedBy)
        {
            if (string.IsNullOrWhiteSpace(deletedBy))
                throw new ArgumentException("DeletedBy cannot be empty", nameof(deletedBy));
            
            if (IsDeleted)
                throw new InvalidOperationException("Entity is already deleted");

            IsDeleted = true;
            DeletedDate = DateTime.UtcNow;
            DeletedBy = new UserId(deletedBy);
            
            _domainEvents.Add(new EntityDeletedEvent(deletedBy));
        }

        public void MarkAsNotDeleted()
        {
            if (!IsDeleted)
                throw new InvalidOperationException("Entity is not deleted");

            IsDeleted = false;
            DeletedDate = null;
            DeletedBy = null;
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
