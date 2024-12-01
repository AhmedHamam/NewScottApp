using Base.Domain.ValueObjects;

namespace Base.Domain.DomainEvents
{
    public class TenantAssignedEvent : IDomainEvent
    {
        public TenantId TenantId { get; }
        public DateTime OccurredOn { get; }

        public TenantAssignedEvent(TenantId tenantId)
        {
            TenantId = tenantId ?? throw new ArgumentNullException(nameof(tenantId));
            OccurredOn = DateTime.UtcNow;
        }
    }
}
