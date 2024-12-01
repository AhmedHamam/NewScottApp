using Base.Domain.CommonInterfaces;
using Base.Domain.ValueObjects;

namespace Base.Domain.CommonModels
{
    /// <summary>
    /// Base class for entities that belong to a specific tenant in a multi-tenant system
    /// </summary>
    /// <remarks>
    /// This class extends AuditableEntity to provide tenant isolation capabilities while
    /// maintaining full audit trail functionality. It implements ITenantEntity interface
    /// and raises appropriate domain events when tenant assignment occurs.
    /// </remarks>
    public abstract class TenantAwareEntity : AuditableEntity, ITenantEntity
    {
        /// <summary>
        /// Gets the unique identifier of the tenant that owns this entity
        /// </summary>
        public TenantId TenantId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantAwareEntity"/> class with a specified tenant
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant</param>
        /// <exception cref="ArgumentNullException">Thrown when tenantId is null</exception>
        protected TenantAwareEntity(TenantId tenantId)
        {
            if (tenantId == null)
                throw new ArgumentNullException(nameof(tenantId));

            TenantId = tenantId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantAwareEntity"/> class
        /// </summary>
        /// <remarks>
        /// This protected constructor is provided for ORM compatibility
        /// </remarks>
        protected TenantAwareEntity() { }

        /// <summary>
        /// Assigns the entity to a specific tenant
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant</param>
        /// <exception cref="ArgumentNullException">Thrown when tenantId is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when the entity is already assigned to a tenant</exception>
        public void AssignToTenant(TenantId tenantId)
        {
            if (tenantId == null)
                throw new ArgumentNullException(nameof(tenantId));

            if (TenantId != null)
                throw new InvalidOperationException("Entity is already assigned to a tenant");

            TenantId = tenantId;
        }
    }
}
