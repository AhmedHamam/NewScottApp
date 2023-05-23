using Base.Domain.CommonInterfaces;

namespace Base.Domain.CommonModels
{
    public class BaseEntity : ISoftDelete
    {
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedDate { get; private set; }
        public string? DeletedBy { get; private set; }
        public void MarkAsDeleted(string deletedBy)
        {
            IsDeleted = true;
            DeletedDate = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }

        public void MarkAsNotDeleted()
        {
            IsDeleted = false;
        }
    }
}
