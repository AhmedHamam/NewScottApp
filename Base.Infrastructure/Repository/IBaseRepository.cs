using System.Linq.Expressions;

namespace Base.Infrastructure.Repository
{
    /// <summary>
    /// Generic repository interface for basic database operations
    /// </summary>
    /// <typeparam name="TEntity">The type of entity this repository works with</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        #region Query Operations

        /// <summary>
        /// Gets an entity by its primary key
        /// </summary>
        /// <param name="id">The primary key value</param>
        /// <returns>The entity if found, null otherwise</returns>
        TEntity? GetById(object id);

        /// <summary>
        /// Gets an entity by its primary key asynchronously
        /// </summary>
        /// <param name="id">The primary key value</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The entity if found, null otherwise</returns>
        Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds entities based on a filter with ordering and include capabilities
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <param name="orderBy">Order by expression</param>
        /// <param name="includeProperties">Comma-separated list of properties to include</param>
        /// <returns>Collection of entities matching the criteria</returns>
        IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Finds entities asynchronously based on a filter with ordering and include capabilities
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <param name="orderBy">Order by expression</param>
        /// <param name="includeProperties">Comma-separated list of properties to include</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Collection of entities matching the criteria</returns>
        Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "",
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>All entities in the repository</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Gets all entities asynchronously
        /// </summary>
        /// <returns>All entities in the repository</returns>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a queryable for custom queries
        /// </summary>
        /// <returns>IQueryable of entities</returns>
        IQueryable<TEntity> Query();

        #endregion

        #region Command Operations

        /// <summary>
        /// Adds a new entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <returns>The added entity</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// Adds a new entity asynchronously
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The added entity</returns>
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a range of entities
        /// </summary>
        /// <param name="entities">Entities to add</param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Adds a range of entities asynchronously
        /// </summary>
        /// <param name="entities">Entities to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        /// <returns>The updated entity</returns>
        TEntity Update(TEntity entityToUpdate);

        /// <summary>
        /// Updates an existing entity asynchronously
        /// </summary>
        /// <param name="entityToUpdate">Entity to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated entity</returns>
        Task<TEntity> UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates a range of entities
        /// </summary>
        /// <param name="entities">Entities to update</param>
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates a range of entities asynchronously
        /// </summary>
        /// <param name="entities">Entities to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        #endregion

        #region Delete Operations

        /// <summary>
        /// Removes an entity by its primary key
        /// </summary>
        /// <param name="id">Primary key of the entity to remove</param>
        void RemoveById(object id);

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="entityToDelete">Entity to remove</param>
        void Remove(TEntity entityToDelete);

        /// <summary>
        /// Removes a range of entities
        /// </summary>
        /// <param name="entities">Entities to remove</param>
        void RemoveRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes an entity by its primary key asynchronously
        /// </summary>
        /// <param name="id">Primary key of the entity to remove</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RemoveByIdAsync(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an entity asynchronously
        /// </summary>
        /// <param name="entityToDelete">Entity to remove</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RemoveAsync(TEntity entityToDelete, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a range of entities asynchronously
        /// </summary>
        /// <param name="entities">Entities to remove</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        #endregion

        #region Soft Delete Operations

        /// <summary>
        /// Soft deletes an entity by its primary key
        /// </summary>
        /// <param name="id">Primary key of the entity to soft delete</param>
        /// <param name="userId">ID of the user performing the operation</param>
        void SoftDelete(object id, string userId);

        /// <summary>
        /// Soft deletes an entity
        /// </summary>
        /// <param name="entityToDelete">Entity to soft delete</param>
        /// <param name="userId">ID of the user performing the operation</param>
        void SoftDelete(TEntity entityToDelete, string userId);

        /// <summary>
        /// Soft deletes a range of entities
        /// </summary>
        /// <param name="entities">Entities to soft delete</param>
        /// <param name="userId">ID of the user performing the operation</param>
        void SoftDelete(IEnumerable<TEntity> entities, string userId);

        /// <summary>
        /// Soft deletes an entity by its primary key asynchronously
        /// </summary>
        /// <param name="id">Primary key of the entity to soft delete</param>
        /// <param name="userId">ID of the user performing the operation</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task SoftDeleteAsync(object id, string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft deletes an entity asynchronously
        /// </summary>
        /// <param name="entityToDelete">Entity to soft delete</param>
        /// <param name="userId">ID of the user performing the operation</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task SoftDeleteAsync(TEntity entityToDelete, string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft deletes a range of entities asynchronously
        /// </summary>
        /// <param name="entities">Entities to soft delete</param>
        /// <param name="userId">ID of the user performing the operation</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task SoftDeleteAsync(IEnumerable<TEntity> entities, string userId, CancellationToken cancellationToken = default);

        #endregion

        #region Save Operations

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of affected records</returns>
        int Save();

        /// <summary>
        /// Saves all pending changes asynchronously
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The number of affected records</returns>
        Task<int> SaveAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}
