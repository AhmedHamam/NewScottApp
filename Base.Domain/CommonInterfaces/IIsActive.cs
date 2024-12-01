using Base.Domain.ValueObjects;

namespace Base.Domain.CommonInterfaces
{
    /// <summary>
    /// Defines an interface for entities that can be activated or deactivated
    /// </summary>
    /// <remarks>
    /// This interface provides functionality to track the active state of an entity,
    /// along with who performed the activation/deactivation and when it occurred.
    /// It's useful for entities that need to be temporarily disabled without being deleted.
    /// </remarks>
    public interface IIsActive
    {
        /// <summary>
        /// Gets a value indicating whether this entity is currently active
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Gets the date and time when the entity's active state was last changed
        /// </summary>
        DateTime? LastActiveStateChange { get; }

        /// <summary>
        /// Gets the identifier of the user who last changed the entity's active state
        /// </summary>
        UserId? LastActiveStateChangedBy { get; }

        /// <summary>
        /// Activates the entity
        /// </summary>
        /// <param name="activatedBy">The identifier of the user activating the entity</param>
        /// <exception cref="ArgumentException">Thrown when activatedBy is null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when the entity is already active</exception>
        void Activate(string activatedBy);

        /// <summary>
        /// Deactivates the entity
        /// </summary>
        /// <param name="deactivatedBy">The identifier of the user deactivating the entity</param>
        /// <exception cref="ArgumentException">Thrown when deactivatedBy is null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when the entity is already inactive</exception>
        void Deactivate(string deactivatedBy);
    }
}
