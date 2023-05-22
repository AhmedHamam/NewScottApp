using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Configuration.Infrastructure
{
    public class ConfigurationsDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("appsettings.json");
            var config = configBuilder.Build();
            var connectionString = config.GetValue<string>("ConnectionStrings:DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var httpContextAccessor = new HttpContextAccessor();

            return new ConfigurationDbContext(optionsBuilder.Options, httpContextAccessor);
        }
    }
}
