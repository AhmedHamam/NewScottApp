using System.Security.Claims;

namespace Base.Application.Interfaces;

/// <summary>
/// Provides access to the current user's information
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the ID of the current user
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Gets the email of the current user
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Gets a value indicating whether the current user is authenticated
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the roles of the current user
    /// </summary>
    IEnumerable<string> Roles { get; }

    /// <summary>
    /// Gets all claims for the current user
    /// </summary>
    IEnumerable<Claim> Claims { get; }

    /// <summary>
    /// Checks if the current user is in the specified role
    /// </summary>
    /// <param name="role">The role to check</param>
    /// <returns>True if the user is in the role, false otherwise</returns>
    bool IsInRole(string role);

    /// <summary>
    /// Gets the value of a specific claim for the current user
    /// </summary>
    /// <param name="claimType">The type of claim to retrieve</param>
    /// <returns>The claim value if found, null otherwise</returns>
    string? GetClaimValue(string claimType);
    Task<string> GetUserNameAsync(string userId);
}
