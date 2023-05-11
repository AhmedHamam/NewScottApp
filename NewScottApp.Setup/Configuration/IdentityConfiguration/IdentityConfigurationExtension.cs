using Microsoft.Extensions.Configuration;

namespace NewScottApp.Setup.Configuration.IdentityConfiguration;

public static class IdentityConfigurationExtension
{
    public static IdentityClientConfig GetAdminClientConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection("IdentityClients:AdminPortal").Get<IdentityClientConfig>();
        if (config is null)
        {
            throw new Exception("Missing 'IdentityClients:AdminPortal' configuration section from the appsettings.");
        }

        return config;
    }

    public static IdentityClientConfig GetPublicClientConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection("IdentityClients:PublicPortal").Get<IdentityClientConfig>();
        if (config is null)
        {
            throw new Exception("Missing 'IdentityClients:AdminPortal' configuration section from the appsettings.");
        }

        return config;
    }
}