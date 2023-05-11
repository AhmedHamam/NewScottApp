namespace Base.Application.Services.RedisCache.Models.Config;

/// <summary>
/// 
/// </summary>
public sealed class RedisConfig
{
    /// <summary>
    /// 
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ConnectionString { get; set; }
}