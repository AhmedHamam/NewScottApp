using Base.Domain.CommonInterfaces;
using Base.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using System.Security.Claims;

namespace Base.Infrastructure.Persistence
{
    /// <summary>
    /// Application's main database context that handles entity tracking, auditing, and soft delete
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string DefaultUserIdClaimType = "sub";
        private readonly Func<IHttpContextAccessor, string?>? _getUserId;
        private readonly string _userIdClaimType;

        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor for user information</param>
        /// <param name="getUserId">Optional custom function to retrieve user ID</param>
        /// <param name="userIdClaimType">Claim type for user ID (default: "sub")</param>
        public ApplicationDbContext(
            IHttpContextAccessor httpContextAccessor,
            Func<IHttpContextAccessor, string?>? getUserId = null,
            string userIdClaimType = DefaultUserIdClaimType)
        {
            _httpContextAccessor = httpContextAccessor;
            _getUserId = getUserId;
            _userIdClaimType = userIdClaimType;
        }

        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext with specific options
        /// </summary>
        /// <param name="options">The options to be used by DbContext</param>
        /// <param name="httpContextAccessor">HTTP context accessor for user information</param>
        /// <param name="getUserId">Optional custom function to retrieve user ID</param>
        /// <param name="userIdClaimType">Claim type for user ID (default: "sub")</param>
        public ApplicationDbContext(
            DbContextOptions options,
            IHttpContextAccessor httpContextAccessor,
            Func<IHttpContextAccessor, string?>? getUserId = null,
            string userIdClaimType = DefaultUserIdClaimType) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _getUserId = getUserId;
            _userIdClaimType = userIdClaimType;
        }

        #region Save Changes

        /// <summary>
        /// Saves changes asynchronously with explicit user ID
        /// </summary>
        /// <param name="userId">The ID of the user making changes</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>The number of affected records</returns>
        public async Task<int> SaveChangesAsync(string userId, CancellationToken cancellationToken = default)
        {
            CheckAndUpdateEntities(userId);
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Saves changes with explicit user ID
        /// </summary>
        /// <param name="userId">The ID of the user making changes</param>
        /// <returns>The number of affected records</returns>
        public int SaveChanges(string userId)
        {
            CheckAndUpdateEntities(userId);
            return base.SaveChanges();
        }

        /// <summary>
        /// Saves changes asynchronously using the current user's ID
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>The number of affected records</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var id = _getUserId?.Invoke(_httpContextAccessor) ?? GetClaimValue(_httpContextAccessor, _userIdClaimType);
            CheckAndUpdateEntities(id);
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Saves changes using the current user's ID
        /// </summary>
        /// <returns>The number of affected records</returns>
        public override int SaveChanges()
        {
            var id = _getUserId?.Invoke(_httpContextAccessor) ?? GetClaimValue(_httpContextAccessor, _userIdClaimType);
            CheckAndUpdateEntities(id);
            return base.SaveChanges();
        }

        #endregion

        /// <summary>
        /// Configures the database model
        /// </summary>
        /// <param name="builder">The model builder</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Set default string length
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(255);
                    }
                }
            }

            // Apply entity configurations
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);

            // Apply global query filter for soft delete
            builder.GetOnlyNotDeletedEntities();
        }

        #region Helper Methods

        /// <summary>
        /// Updates entity states and audit information
        /// </summary>
        /// <param name="userId">The ID of the user making changes</param>
        protected virtual void CheckAndUpdateEntities(string? userId)
        {
            // Handle created entities
            foreach (var entry in ChangeTracker.Entries<ICreatedAuditableEntity>()
                .Where(e => e.State == EntityState.Added))
            {
                entry.Entity.MarkAsCreated(userId);
            }

            // Handle modified entities
            foreach (var entry in ChangeTracker.Entries<IModifiedAuditableEntity>()
                .Where(e => e.State == EntityState.Modified))
            {
                entry.Entity.MarkAsUpdated(userId);
            }

            // Handle soft delete
            foreach (var entry in ChangeTracker.Entries<ISoftDelete>()
                .Where(e => e.State is EntityState.Added or EntityState.Deleted))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.MarkAsNotDeleted();
                }
                else
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.MarkAsDeleted(userId);
                }
            }
        }

        /// <summary>
        /// Gets a claim value from the current user
        /// </summary>
        /// <param name="accessor">The HTTP context accessor</param>
        /// <param name="claimType">The type of claim to retrieve</param>
        /// <returns>The claim value or null if not found</returns>
        protected virtual string? GetClaimValue(IHttpContextAccessor accessor, string claimType)
        {
            var user = accessor?.HttpContext?.User;
            if (user?.Identity is null || !user.Identity.IsAuthenticated)
                return null;

            return user.FindFirstValue(claimType);
        }

        #endregion
    }
}
