namespace Base.Domain.CommonInterfaces
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; }
        public DateTime? DeletedDate { get; }
        public string? DeletedBy { get; }
        public void MarkAsDeleted(string deletedBy);
        public void MarkAsNotDeleted();
    }
}
