namespace Base.API.Variables;

/// <summary>
/// 
/// </summary>
public static class EnvVariables
{
    /// <summary>
    /// 
    /// </summary>
    public static string? ENVIRONMENT_NAME = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
}