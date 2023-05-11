namespace Base.Domain.CommonInterfaces
{
    public interface ISoftDelete<T>
    {
        public bool IsDeleted { get; }
        public DateTimeOffset? DeletedOn { get; }
        public T DeletedBy { get; }
        public void MarkAsDeleted(T deletedBy);
        public void MarkAsNotDeleted();
    }
}
