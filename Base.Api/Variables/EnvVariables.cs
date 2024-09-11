namespace Base.API.Variables;

/// <summary>
/// 
/// </summary>
public static class EnvVariables
{
    //TODO : Check what is this 
    /// <summary>
    /// 
    /// </summary>
    public static string? ENVIRONMENT_NAME = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
}
