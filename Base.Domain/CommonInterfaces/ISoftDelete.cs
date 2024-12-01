using Base.Domain.ValueObjects;

namespace Base.Domain.CommonInterfaces
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; }
        public DateTime? DeletedDate { get; }
        public UserId DeletedBy { get; }
        public void MarkAsDeleted(string deletedBy);
        public void MarkAsNotDeleted();
    }
}
