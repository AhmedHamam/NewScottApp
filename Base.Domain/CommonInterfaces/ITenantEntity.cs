using Base.Domain.ValueObjects;

namespace Base.Domain.CommonInterfaces
{
    public interface ITenantEntity
    {
        TenantId TenantId { get; }
        void AssignToTenant(TenantId tenantId);
    }
}
