using Base.Domain.CommonInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.Infrastructure.Extensions
{

    public static class SoftDeleteQueryExtension
    {
        public static void GetOnlyNotDeletedEntities(this ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                //other automated configurations left out
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }
        }

        private static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteQueryExtension)
                ?.GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(entityData.ClrType);

            var filter = methodToCall?.Invoke(null, Array.Empty<object>());

            if (filter == null) return;
            entityData.SetQueryFilter((LambdaExpression)filter);

            entityData.AddIndex(entityData.FindProperty(nameof(ISoftDelete.IsDeleted)) ??
                                throw new InvalidOperationException());
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity : class, ISoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }
    }
}