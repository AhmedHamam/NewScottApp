using Base.Domain.CommonInterfaces;
using Base.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Base.Infrastructure.Repository
{
    /// <summary>
    /// Generic repository implementation for basic database operations
    /// </summary>
    /// <typeparam name="TEntity">The type of entity this repository works with</typeparam>
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _set;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = _context.Set<TEntity>();
        }

        #region Query Operations

        public virtual TEntity? GetById(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return _set.Find(id);
        }

        public virtual async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return await _set.FindAsync(new[] { id }, cancellationToken);
        }

        public virtual IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _set;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "",
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _set;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            return orderBy != null
                ? await orderBy(query).ToListAsync(cancellationToken)
                : await query.ToListAsync(cancellationToken);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _set.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _set.ToListAsync(cancellationToken);
        }

        public virtual IQueryable<TEntity> Query()
        {
            return _set;
        }

        #endregion

        #region Command Operations

        public virtual TEntity Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _set.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _set.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _set.AddRange(entities);
            _context.SaveChanges();
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            await _set.AddRangeAsync(entities, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual TEntity Update(TEntity entityToUpdate)
        {
            if (entityToUpdate == null)
                throw new ArgumentNullException(nameof(entityToUpdate));

            _set.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
            return entityToUpdate;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken = default)
        {
            if (entityToUpdate == null)
                throw new ArgumentNullException(nameof(entityToUpdate));

            _set.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return entityToUpdate;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Delete Operations

        public virtual void RemoveById(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entityToDelete = _set.Find(id) ?? 
                throw new InvalidOperationException($"Entity with ID {id} not found");
            _set.Remove(entityToDelete);
            _context.SaveChanges();
        }

        public virtual void Remove(TEntity entityToDelete)
        {
            if (entityToDelete == null)
                throw new ArgumentNullException(nameof(entityToDelete));

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _set.Attach(entityToDelete);
            }
            _set.Remove(entityToDelete);
            _context.SaveChanges();
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _set.RemoveRange(entities);
            _context.SaveChanges();
        }

        public virtual async Task RemoveByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entityToDelete = await _set.FindAsync(new[] { id }, cancellationToken) ?? 
                throw new InvalidOperationException($"Entity with ID {id} not found");
            _set.Remove(entityToDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task RemoveAsync(TEntity entityToDelete, CancellationToken cancellationToken = default)
        {
            if (entityToDelete == null)
                throw new ArgumentNullException(nameof(entityToDelete));

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _set.Attach(entityToDelete);
            }
            _set.Remove(entityToDelete);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _set.RemoveRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Soft Delete Operations

        public virtual void SoftDelete(object id, string userId)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            var entity = _set.Find(id) ?? 
                throw new InvalidOperationException($"Entity with ID {id} not found");
            
            if (entity is ISoftDelete deletable)
            {
                deletable.MarkAsDeleted(userId);
                _set.Update(entity);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} does not implement ISoftDelete");
            }
        }

        public virtual void SoftDelete(TEntity entityToDelete, string userId)
        {
            if (entityToDelete == null)
                throw new ArgumentNullException(nameof(entityToDelete));
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            if (entityToDelete is ISoftDelete deletable)
            {
                deletable.MarkAsDeleted(userId);
                _set.Update(entityToDelete);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} does not implement ISoftDelete");
            }
        }

        public virtual void SoftDelete(IEnumerable<TEntity> entities, string userId)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            foreach (var entity in entities)
            {
                if (entity is ISoftDelete deletable)
                {
                    deletable.MarkAsDeleted(userId);
                    _set.Update(entity);
                }
                else
                {
                    throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} does not implement ISoftDelete");
                }
            }
            _context.SaveChanges();
        }

        public virtual async Task SoftDeleteAsync(object id, string userId, CancellationToken cancellationToken = default)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            var entity = await _set.FindAsync(new[] { id }, cancellationToken) ?? 
                throw new InvalidOperationException($"Entity with ID {id} not found");
            
            if (entity is ISoftDelete deletable)
            {
                deletable.MarkAsDeleted(userId);
                _set.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} does not implement ISoftDelete");
            }
        }

        public virtual async Task SoftDeleteAsync(TEntity entityToDelete, string userId, CancellationToken cancellationToken = default)
        {
            if (entityToDelete == null)
                throw new ArgumentNullException(nameof(entityToDelete));
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            if (entityToDelete is ISoftDelete deletable)
            {
                deletable.MarkAsDeleted(userId);
                _set.Update(entityToDelete);
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} does not implement ISoftDelete");
            }
        }

        public virtual async Task SoftDeleteAsync(IEnumerable<TEntity> entities, string userId, CancellationToken cancellationToken = default)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty", nameof(userId));

            foreach (var entity in entities)
            {
                if (entity is ISoftDelete deletable)
                {
                    deletable.MarkAsDeleted(userId);
                    _set.Update(entity);
                }
                else
                {
                    throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} does not implement ISoftDelete");
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Save Operations

        public virtual int Save()
        {
            return _context.SaveChanges();
        }

        public virtual async Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        #endregion
    }
}
