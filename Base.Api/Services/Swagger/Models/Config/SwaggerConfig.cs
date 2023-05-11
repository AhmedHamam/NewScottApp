namespace Base.API.Services.Swagger.Models.Config;

/// <summary>
/// 
/// </summary>
public sealed class SwaggerConfig
{
    /// <summary>
    /// 
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool HideModels { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool DocumentationEnabled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Title { get; set; }
}