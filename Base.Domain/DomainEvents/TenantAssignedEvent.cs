using Base.Domain.ValueObjects;

namespace Base.Domain.DomainEvents
{
    /// <summary>
    /// Represents a domain event that is raised when an entity is assigned to a tenant
    /// </summary>
    /// <remarks>
    /// This event is typically raised by entities implementing the ITenantEntity interface
    /// when they are assigned to a tenant. It carries information about the tenant assignment
    /// and when it occurred.
    /// </remarks>
    public class TenantAssignedEvent : IDomainEvent
    {
        /// <summary>
        /// Gets the unique identifier of the tenant that the entity was assigned to
        /// </summary>
        public TenantId TenantId { get; }

        /// <summary>
        /// Gets the date and time when this domain event occurred
        /// </summary>
        public DateTime OccurredOn { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantAssignedEvent"/> class
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant</param>
        /// <exception cref="ArgumentNullException">Thrown when tenantId is null</exception>
        public TenantAssignedEvent(TenantId tenantId)
        {
            TenantId = tenantId ?? throw new ArgumentNullException(nameof(tenantId));
            OccurredOn = DateTime.UtcNow;
        }
    }
}
