namespace Base.Domain.CommonInterfaces
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; }
        public DateTimeOffset? DeletedDate { get; }
        public string DeletedBy { get; }
        public void MarkAsDeleted(string deletedBy);
        public void MarkAsNotDeleted();
    }
}
