using Base.Domain.CommonInterfaces;
using Base.Domain.ValueObjects;

namespace Base.Domain.CommonModels
{
    public abstract class TenantAwareEntity : AuditableEntity, ITenantEntity
    {
        public TenantId TenantId { get; private set; }

        protected TenantAwareEntity(TenantId tenantId)
        {
            if (tenantId == null)
                throw new ArgumentNullException(nameof(tenantId));

            TenantId = tenantId;
        }

        protected TenantAwareEntity() { } // For ORM

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
