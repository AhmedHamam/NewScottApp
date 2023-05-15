using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NewScotApp.Setup.Configuration.IdentityConfiguration;

namespace NewScotApp.Setup.Constants;

public static class IdentitySettings
{
    public static PasswordOptions PasswordOptions()
    {
        return new PasswordOptions()
        {
            RequireDigit = true,
            RequireNonAlphanumeric = true,
            RequireLowercase = true,
            RequireUppercase = true,
            RequiredLength = 6,
            RequiredUniqueChars = 5,
        };
    }

    public static LockoutOptions LockoutOptions(IConfiguration configuration)
    {
        return new LockoutOptions()
        {
            DefaultLockoutTimeSpan = TimeSpan.FromHours(10),
            MaxFailedAccessAttempts = configuration.GetAdminClientConfig().Jwt.MaxFailedAccessAttempts,
            AllowedForNewUsers = true,
        };
    }

    public static UserOptions UserOptions()
    {
        return new UserOptions()
        {
            RequireUniqueEmail = true,
        };
    }

    public static SignInOptions SignInOptions()
    {
        return new SignInOptions()
        {
            RequireConfirmedEmail = false,
            RequireConfirmedPhoneNumber = false,
        };
    }
}