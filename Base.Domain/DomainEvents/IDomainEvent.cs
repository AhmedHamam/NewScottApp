namespace Base.Domain.DomainEvents
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
