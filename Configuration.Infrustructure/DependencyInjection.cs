using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConfigurationsInfrastructure(this IServiceCollection services,
    IConfiguration configuration)
        {
            //services.AddTransient<ApplicationDbContext, ConfigurationsDbContext>();
            //services.AddScoped<IAlertClassificationRepository, AlertClassificationRepository>();
            //services.AddScoped<ICountryCommandRepository, CountryCommandRepository>();
            //services.AddScoped<ICountryQueryRepository, CountryQueryRepository>();
            services.AddDbContext<ConfigurationsDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ConfigurationsDbContext).Assembly.FullName)), ServiceLifetime.Transient);

            return services;
        }
    }
}
