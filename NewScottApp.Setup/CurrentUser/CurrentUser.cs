using Microsoft.AspNetCore.Http;

namespace NewScotApp.Setup.CurrentUser;

public static class CurrentUser
{
    private static IHttpContextAccessor _httpContextAccessor;

    #region Logged In User Claims

    public static Guid? Id => GetClaimValue(ClaimKeys.Id) is not null ? Guid.Parse(GetClaimValue(ClaimKeys.Id)!) : null;
    public static string? Email => GetClaimValue(ClaimKeys.Email);

    //  public static List<RolesEnum> Roles => GetRoles();

    #endregion

    #region Helper Methods

    private static string? GetClaimValue(string key)
    {
        var user = _httpContextAccessor?.HttpContext?.User;
        if (user?.Identity is null || !user.Identity.IsAuthenticated) return null;

        var value = user?.Claims?.FirstOrDefault(x => x.Type == key)?.Value;
        return value;
    }

    //private static List<RolesEnum>? GetRoles()
    //{
    //    var user = _httpContextAccessor?.HttpContext?.User;

    //    if (user?.Identity is null || user.Identity.IsAuthenticated == false) return null;

    //    var roles = user?.Claims?
    //        .Where(x => x.Type == ClaimTypes.Role)
    //        .Select(x => Enum.Parse<RolesEnum>(x.Value))
    //        .ToList();

    //    return roles;
    //}

    #endregion

    internal static void InitializeHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}