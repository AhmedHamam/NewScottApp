using Base.Domain.CommonInterfaces;

namespace Base.Domain.CommonModels
{
    public class BaseEntity<T> : ISoftDelete<T>
    {
        public T Id { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeletedOn { get; private set; }
        public T DeletedBy { get; private set; }
        public void MarkAsDeleted(T deletedBy)
        {
            IsDeleted = true;
            DeletedOn = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }

        public void MarkAsNotDeleted()
        {
            IsDeleted = false;
        }
    }
}
