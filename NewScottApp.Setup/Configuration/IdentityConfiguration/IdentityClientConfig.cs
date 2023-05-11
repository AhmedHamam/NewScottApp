namespace NewScottApp.Setup.Configuration.IdentityConfiguration;

public class IdentityClientConfig
{
    public string BackendUrl { get; set; }
    public string WebUrl { get; set; }
    public IdentityClientJwtConfig Jwt { get; set; }
}

public class IdentityClientJwtConfig
{
    public string Issuer { get; set; }
    public int MaxFailedAccessAttempts { get; set; }
    public string Key { get; set; }
    public string Audience { get; set; }
    public int ExpiryDuration { get; set; }
}