namespace Base.Domain.CommonInterfaces
{
    public interface IIsActive<T>
    {
        public bool IsActive { get; }
        public void MarkAsActive(T activatedBy);
        public void MarkAsNotActive(T deactivatedBy);
    }
}
