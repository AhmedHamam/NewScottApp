using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOrderInfrastructure(this IServiceCollection services,
    IConfiguration configuration)
        {
            services.AddScoped<ConfigurationDbContext>();

            services.AddDbContext<ConfigurationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ConfigurationDbContext).Assembly.FullName)));

            return services;
        }
    }
}
