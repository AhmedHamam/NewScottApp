using Base.Infrastructure.Persistence;
using Base.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Base.Infrastructure
{
    /// <summary>
    /// Extension methods for configuring infrastructure services in the dependency injection container
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds and configures infrastructure services to the service collection
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to</param>
        /// <param name="configuration">The application configuration</param>
        /// <param name="connectionStringName">The name of the connection string in configuration (default: "DefaultConnection")</param>
        /// <returns>The IServiceCollection for chaining</returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionStringName = "DefaultConnection")
        {
            // Add DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString(connectionStringName),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    }));

            // Register repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            // Add other infrastructure services
            services.AddMemoryCache();
            
            // Add health checks
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(
                    name: "Database",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "db", "sql", "sqlserver" });

            return services;
        }

        /// <summary>
        /// Adds infrastructure services with custom database configuration
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to</param>
        /// <param name="optionsAction">Action to configure the DbContext</param>
        /// <returns>The IServiceCollection for chaining</returns>
        public static IServiceCollection AddInfrastructureWithCustomDb(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            // Add DbContext with custom configuration
            services.AddDbContext<ApplicationDbContext>(optionsAction);

            // Register repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            // Add other infrastructure services
            services.AddMemoryCache();
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(
                    name: "Database",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "db", "sql", "sqlserver" });

            return services;
        }
    }
}
