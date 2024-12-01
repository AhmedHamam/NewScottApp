using Base.Domain.ValueObjects;

namespace Base.Domain.CommonInterfaces
{
    /// <summary>
    /// Defines an interface for entities that belong to a specific tenant in a multi-tenant system
    /// </summary>
    public interface ITenantEntity
    {
        /// <summary>
        /// Gets the unique identifier of the tenant that owns this entity
        /// </summary>
        TenantId TenantId { get; }

        /// <summary>
        /// Assigns the entity to a specific tenant
        /// </summary>
        /// <param name="tenantId">The unique identifier of the tenant</param>
        /// <exception cref="ArgumentNullException">Thrown when tenantId is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when entity is already assigned to a tenant</exception>
        void AssignToTenant(TenantId tenantId);
    }
}
