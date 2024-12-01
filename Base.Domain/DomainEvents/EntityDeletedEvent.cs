namespace Base.Domain.DomainEvents
{
    public class EntityDeletedEvent : IDomainEvent
    {
        public string DeletedBy { get; }
        public DateTime DeletedDate { get; }
        public DateTime OccurredOn { get; }

        public EntityDeletedEvent(string deletedBy)
        {
            DeletedBy = deletedBy;
            DeletedDate = DateTime.UtcNow;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
