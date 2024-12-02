using Base.Domain.CommonInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods for implementing soft delete functionality in Entity Framework Core
    /// </summary>
    public static class SoftDeleteQueryExtension
    {
        /// <summary>
        /// Configures global query filters for soft-deleted entities in the model
        /// </summary>
        /// <param name="builder">The model builder instance</param>
        /// <remarks>
        /// This method adds a global query filter to all entities implementing ISoftDelete,
        /// ensuring that soft-deleted entities are automatically filtered out of queries
        /// unless explicitly included.
        /// </remarks>
        public static void GetOnlyNotDeletedEntities(this ModelBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }

        /// <summary>
        /// Adds a soft delete query filter to an entity type
        /// </summary>
        /// <param name="entityData">The entity type to configure</param>
        /// <exception cref="InvalidOperationException">Thrown when the IsDeleted property cannot be found on the entity</exception>
        private static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteQueryExtension)
                .GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(entityData.ClrType);

            if (methodToCall == null)
                throw new InvalidOperationException($"Failed to get {nameof(GetSoftDeleteFilter)} method for type {entityData.ClrType.Name}");

            var filter = methodToCall.Invoke(null, Array.Empty<object>());
            if (filter == null)
                throw new InvalidOperationException($"Failed to create filter for type {entityData.ClrType.Name}");

            entityData.SetQueryFilter((LambdaExpression)filter);

            var isDeletedProperty = entityData.FindProperty(nameof(ISoftDelete.IsDeleted));
            if (isDeletedProperty == null)
                throw new InvalidOperationException($"Property {nameof(ISoftDelete.IsDeleted)} not found on entity {entityData.ClrType.Name}");

            entityData.AddIndex(isDeletedProperty);
        }

        /// <summary>
        /// Creates a filter expression for soft-deleted entities
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to filter</typeparam>
        /// <returns>A lambda expression representing the soft delete filter</returns>
        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity : class, ISoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }

        /// <summary>
        /// Includes soft-deleted entities in a query
        /// </summary>
        /// <typeparam name="TEntity">The type of entity</typeparam>
        /// <param name="query">The query to modify</param>
        /// <returns>A query including soft-deleted entities</returns>
        public static IQueryable<TEntity> IncludeSoftDeleted<TEntity>(this IQueryable<TEntity> query)
            where TEntity : class, ISoftDelete
        {
            return query.IgnoreQueryFilters();
        }

        /// <summary>
        /// Gets only soft-deleted entities from a query
        /// </summary>
        /// <typeparam name="TEntity">The type of entity</typeparam>
        /// <param name="query">The query to modify</param>
        /// <returns>A query returning only soft-deleted entities</returns>
        public static IQueryable<TEntity> OnlySoftDeleted<TEntity>(this IQueryable<TEntity> query)
            where TEntity : class, ISoftDelete
        {
            return query.IgnoreQueryFilters().Where(e => e.IsDeleted);
        }
    }
}