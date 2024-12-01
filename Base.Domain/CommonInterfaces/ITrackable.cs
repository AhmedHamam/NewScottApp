namespace Base.Domain.CommonInterfaces
{
    /// <summary>
    /// Defines an interface for entities that need version tracking and concurrency control
    /// </summary>
    public interface ITrackable
    {
        /// <summary>
        /// Gets the version number of the entity, incremented on each update
        /// </summary>
        int Version { get; }

        /// <summary>
        /// Gets the concurrency stamp used for optimistic concurrency control
        /// </summary>
        string ConcurrencyStamp { get; }

        /// <summary>
        /// Increments the version number and updates the concurrency stamp
        /// </summary>
        void IncrementVersion();
    }
}
