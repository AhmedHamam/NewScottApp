using Base.Domain.CommonInterfaces;
using Base.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Base.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _set;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            _set.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _set.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public void AddRange(List<TEntity> entities)
        {
            _set.AddRange(entities);
            _context.SaveChanges();
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _set.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync(cancellationToken);
            }
            else
            {
                return await query.ToListAsync(cancellationToken);
            }
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _set.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public TEntity? GetById(object id)
        {
            return _set.Find(id);
        }
        public async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await _set.FindAsync(new object[] { id }, cancellationToken);
        }

        public void Remove(TEntity entityToDelete)
        {
            _set.Remove(entityToDelete);
            _context.SaveChanges();
        }
        public async Task RemoveAsync(TEntity entityToDelete, CancellationToken cancellationToken)
        {
            _set.Remove(entityToDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void RemoveById(object id)
        {
            TEntity entityToDelete = _set.Find(id);
            _set.Remove(entityToDelete);
            _context.SaveChanges();
        }
        public async Task RemoveByIdAsync(object id, CancellationToken cancellationToken)
        {
            TEntity entityToDelete = await _set.FindAsync(new object[] { id }, cancellationToken);
            _set.Remove(entityToDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void RemoveRange(IEnumerable<TEntity> entitiesToDelete)
        {
            _set.RemoveRange(entitiesToDelete);
            _context.SaveChanges();
        }
        public async Task RemoveRangeAsync(IEnumerable<TEntity> entitiesToDelete, CancellationToken cancellationToken)
        {
            _set.RemoveRange(entitiesToDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public void SofDelete(IEnumerable<TEntity> entitiesToDelete, string userId)
        {
            foreach (var entity in entitiesToDelete)
            {
                if (entity is ISoftDelete deletable)
                {
                    deletable.MarkAsDeleted(userId);
                    _set.Update(entity);
                }
            }
            _context.SaveChanges();
        }
        public void SoftDelete(object id, string userId)
        {
            var entity = _set.Find(id);
            if (entity is ISoftDelete deletable)
            {
                deletable.MarkAsDeleted(userId);

                _set.Update(entity);
            }
            _context.SaveChanges();
        }
        public void SoftDelete(TEntity entityToDelete, string userId)
        {
            if (entityToDelete is ISoftDelete deletable)
            {
                deletable.MarkAsDeleted(userId);
                _set.Update(entityToDelete);
            }
            _context.SaveChanges();
        }

        public async Task SoftDeleteAsync(object id, string userId, CancellationToken cancellationToken)
        {
            var entity = await _set.FindAsync(new object[] { id }, cancellationToken);
            if (entity is ISoftDelete deletable)
            {
                deletable.MarkAsDeleted(userId);
                _set.Update(entity);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task SoftDeleteAsync(TEntity entityToDelete, string userId, CancellationToken cancellationToken)
        {
            if (entityToDelete is ISoftDelete deletable)
            {
                deletable.MarkAsDeleted(userId);
                _set.Update(entityToDelete);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task SoftDeleteAsync(IEnumerable<TEntity> entitiesToDelete, string userId, CancellationToken cancellationToken)
        {
            foreach (var entity in entitiesToDelete)
            {
                if (entity is ISoftDelete deletable)
                {
                    deletable.MarkAsDeleted(userId);
                    _set.Update(entity);
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public TEntity Update(TEntity entityToUpdate)
        {
            _set.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return entityToUpdate;
        }
        public async Task<TEntity> UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken)
        {
            _set.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return entityToUpdate;
        }

    }
}
