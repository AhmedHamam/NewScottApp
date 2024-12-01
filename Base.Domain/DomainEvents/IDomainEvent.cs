namespace Base.Domain.DomainEvents
{
    /// <summary>
    /// Defines the base contract for all domain events in the system
    /// </summary>
    /// <remarks>
    /// Domain events represent significant state changes or business rules that have occurred
    /// within the domain. They are used to maintain loose coupling between domain objects
    /// and to facilitate event-driven architectures.
    /// </remarks>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the date and time when this domain event occurred
        /// </summary>
        DateTime OccurredOn { get; }
    }
}
