using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewScottApp.Domain.Domains.User;
using NewScottApp.Infrastructure;

namespace NewScottApp.Getway.Helpers;

/// <summary>
/// Extensions helpers method for database
/// </summary>
public static class DatabaseExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="scope"></param>
    public static async Task MigrateDatabase(this IServiceScope scope)
    {
        var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDatabaseContext>();
        await identityDbContext.Database.MigrateAsync();
    }

    /// <summary>
    /// </summary>
    /// <param name="scope"></param>
    public static async Task SeedDatabase(this IServiceScope scope)
    {
        var identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDatabaseContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        await IdentityDbContextSeed.SeedDataAsync(identityDbContext, userManager);
    }
}