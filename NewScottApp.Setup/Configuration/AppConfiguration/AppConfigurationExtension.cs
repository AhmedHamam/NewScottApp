using Microsoft.Extensions.Configuration;

namespace NewScotApp.Setup.Configuration.AppConfiguration;

public static class AppConfigurationExtension
{
    public static ApplicationInfoConfig GetApplicationInfoConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection("ApplicationInfo").Get<ApplicationInfoConfig>();
        if (config is null)
        {
            throw new Exception("Missing 'ApplicationInfo' configuration section from the appsettings.");
        }

        return config;
    }
}