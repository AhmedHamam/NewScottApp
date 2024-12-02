using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Base.Application.Interfaces;

/// <summary>
/// Defines the interface for the application's database context
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Gets the database facade for the context
    /// </summary>
    DatabaseFacade Database { get; }

    /// <summary>
    /// Gets the change tracker for the context
    /// </summary>
    ChangeTracker ChangeTracker { get; }

    /// <summary>
    /// Saves all changes made in this context to the database
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete</param>
    /// <returns>The number of state entries written to the database</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets a DbSet<T> that can be used to query and save instances of TEntity
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for which a set should be returned</typeparam>
    /// <returns>A set for the given entity type</returns>
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <summary>
    /// Begins tracking the given entity in the Added state such that it will be inserted into the database
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to add</typeparam>
    /// <param name="entity">The entity to add</param>
    /// <returns>The EntityEntry for the entity</returns>
    EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Begins tracking the given entity in the Modified state such that it will be updated in the database
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to update</typeparam>
    /// <param name="entity">The entity to update</param>
    /// <returns>The EntityEntry for the entity</returns>
    EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Begins tracking the given entity in the Deleted state such that it will be removed from the database
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to remove</typeparam>
    /// <param name="entity">The entity to remove</param>
    /// <returns>The EntityEntry for the entity</returns>
    EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
}