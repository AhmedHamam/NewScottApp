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
    public static Task MigrateDatabase(this IServiceScope scope)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// </summary>
    /// <param name="scope"></param>
    public static Task SeedDatabase(this IServiceScope scope)
    {
        return Task.CompletedTask;
    }
}