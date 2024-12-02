using Base.Infrastructure.Persistence;

namespace Base.Infrastructure.Repository
{
    /// <summary>
    /// Concrete implementation of the generic repository pattern
    /// </summary>
    /// <typeparam name="TEntity">The type of entity this repository works with</typeparam>
    public class Repository<TEntity> : BaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the Repository class
        /// </summary>
        /// <param name="context">The database context</param>
        public Repository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
