namespace Base.Domain.CommonInterfaces
{
    public interface IModifiedAuditableEntity
    {
        public string? UpdatedBy { get; }
        public DateTime? UpdatedDate { get; }
        public bool? IsUpdated { get; }
        public void MarkAsUpdated(string modifiedBy);
    }
}
