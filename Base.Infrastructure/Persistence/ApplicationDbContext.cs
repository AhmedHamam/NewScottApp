﻿using Base.Domain.CommonInterfaces;
using Base.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Base.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string UserId = "sub";
        private readonly Func<IHttpContextAccessor, string?>? _getUserId;

        public ApplicationDbContext(IHttpContextAccessor httpContextAccessor,
            Func<IHttpContextAccessor, string?>? getUserId = null)
        {
            _httpContextAccessor = httpContextAccessor;
            _getUserId = getUserId;
        }

        public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor,
            Func<IHttpContextAccessor, string?>? getUserId = null) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _getUserId = getUserId;
        }


        #region Save Changes

        public async Task<int> SaveChangesAsync(string userId, CancellationToken cancellationToken = new())
        {
            CheckAndUpdateEntities(userId);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public int SaveChanges(string userId)
        {
            CheckAndUpdateEntities(userId);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var id = _getUserId?.Invoke(_httpContextAccessor) ?? GetClaimValue(_httpContextAccessor, UserId);
            CheckAndUpdateEntities(id);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            var id = _getUserId?.Invoke(_httpContextAccessor) ?? GetClaimValue(_httpContextAccessor, UserId);
            CheckAndUpdateEntities(id);
            return base.SaveChanges();
        }

        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        property.SetMaxLength(255);
                    }
                }
            }
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);

            builder.GetOnlyNotDeletedEntities();
        }

        #region Helper Methods

        public void CheckAndUpdateEntities(string? userId)
        {
            ChangeTracker
                .Entries<ICreatedAuditableEntity>()
                .Where(e => e.State == EntityState.Added)
                .ToList().ForEach(entry => { entry.Entity.MarkAsCreated(userId); });
            ChangeTracker
                .Entries<IModifiedAuditableEntity>()
                .Where(e => e.State == EntityState.Modified)
                .ToList().ForEach(entry => { entry.Entity.MarkAsUpdated(userId); });

            ChangeTracker
                .Entries<ISoftDelete>()
                .Where(e => e.State is EntityState.Added or EntityState.Deleted)
                .ToList().ForEach(entry =>
                {
                    if (entry.State == EntityState.Added)
                        entry.Entity.MarkAsNotDeleted();
                    else
                    {
                        entry.State = EntityState.Modified;
                        entry.Entity.MarkAsDeleted(userId);
                    }
                });
        }

        private static string? GetClaimValue(IHttpContextAccessor accessor, string key)
        {
            var user = accessor?.HttpContext?.User;
            if (user?.Identity is null || !user.Identity.IsAuthenticated) return null;

            var value = user.Claims.FirstOrDefault(x => x.Type == key)?.Value;
            return value;
        }

        #endregion

    }
}
