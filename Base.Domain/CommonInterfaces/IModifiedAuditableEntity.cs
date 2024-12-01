using Base.Domain.ValueObjects;

namespace Base.Domain.CommonInterfaces
{
    public interface IModifiedAuditableEntity
    {
        public UserId UpdatedBy { get; }
        public DateTime? UpdatedDate { get; }
        public bool? IsUpdated { get; }
        public void MarkAsUpdated(string modifiedBy);
    }
}
