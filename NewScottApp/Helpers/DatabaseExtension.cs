using Configuration.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace NewScotApp.Getway.Helpers;

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
        var configurationsDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationsDbContext>();
        try
        {
            await configurationsDbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {


        }
    }

    /// <summary>
    /// </summary>
    /// <param name="scope"></param>
    public static Task SeedDatabase(this IServiceScope scope)
    {
        return Task.CompletedTask;
    }
}