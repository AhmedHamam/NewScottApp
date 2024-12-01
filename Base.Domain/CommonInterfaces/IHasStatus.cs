namespace Base.Domain.CommonInterfaces
{
    /// <summary>
    /// Defines an interface for entities that maintain a status
    /// </summary>
    /// <typeparam name="TStatus">The type of the status enum</typeparam>
    public interface IHasStatus<TStatus> where TStatus : struct
    {
        /// <summary>
        /// Gets the current status of the entity
        /// </summary>
        TStatus Status { get; }

        /// <summary>
        /// Updates the status of the entity
        /// </summary>
        /// <param name="newStatus">The new status to set</param>
        /// <param name="updatedBy">The user who is updating the status</param>
        /// <exception cref="ArgumentException">Thrown when updatedBy is null or empty</exception>
        /// <exception cref="InvalidOperationException">Thrown when the status transition is not allowed</exception>
        void UpdateStatus(TStatus newStatus, string updatedBy);
    }
}
