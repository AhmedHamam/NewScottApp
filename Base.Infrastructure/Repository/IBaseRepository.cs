using System.Linq.Expressions;
namespace Base.Infrastructure.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        TEntity? GetById(object id);
        Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

        IEnumerable<TEntity> Find
            (Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        Task<IEnumerable<TEntity>> FindAsync
            (Expression<Func<TEntity, bool>> filter = default,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", CancellationToken cancellationToken = default);

        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();



        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);

        void AddRange(List<TEntity> entity);
        Task AddRangeAsync(List<TEntity> entity);

        TEntity Update(TEntity entityToUpdate);
        Task<TEntity> UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken = default);


        void RemoveById(object id);
        void Remove(TEntity entityToDelete);
        void RemoveRange(IEnumerable<TEntity> entity);

        Task RemoveByIdAsync(object id, CancellationToken cancellationToken = default);
        Task RemoveAsync(TEntity entityToDelete, CancellationToken cancellationToken = default);
        Task RemoveRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken = default);

        void SoftDelete(object id, string userId);
        void SoftDelete(TEntity entityToDelete, string userId);
        void SofDelete(IEnumerable<TEntity> entity, string userId);

        Task SoftDeleteAsync(object id, string userId, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(TEntity entityToDelete, string userId, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(IEnumerable<TEntity> entity, string userId, CancellationToken cancellationToken = default);

        int Save();
        Task<int> SaveAsync();
    }
}
