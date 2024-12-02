using Base.Application.Interfaces;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Security.Claims;

namespace Base.Application.Behaviors;

/// <summary>
/// Pre-processor behavior that logs incoming requests with user context
/// </summary>
/// <typeparam name="TRequest">The type of request being processed</typeparam>
/// <remarks>
/// This behavior logs request details including:
/// - Request name and type
/// - User information (if available)
/// - Request parameters
/// - Timestamp
/// </remarks>
public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest> 
    where TRequest : notnull
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the LoggingBehavior class
    /// </summary>
    /// <param name="httpContextAccessor">HTTP context accessor for user information</param>
    /// <param name="currentUserService">Service to get current user details</param>
    public LoggingBehavior(
        IHttpContextAccessor httpContextAccessor,
        ICurrentUserService currentUserService)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
    }

    /// <summary>
    /// Processes the request by logging its details
    /// </summary>
    /// <param name="request">The request being processed</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;
        var requestInfo = GetRequestInfo(request);
        var userInfo = await GetUserInfo();

        Log.Information(
            "Request Details: {RequestInfo} User Context: {UserInfo} Request Data: {@Request}",
            requestInfo,
            userInfo,
            request);
    }

    /// <summary>
    /// Gets detailed information about the request
    /// </summary>
    private static Dictionary<string, string> GetRequestInfo(TRequest request)
    {
        var requestType = typeof(TRequest);
        return new Dictionary<string, string>
        {
            ["Name"] = requestType.Name,
            ["Type"] = requestType.FullName ?? "Unknown",
            ["Timestamp"] = DateTime.UtcNow.ToString("O")
        };
    }

    /// <summary>
    /// Gets information about the current user
    /// </summary>
    private async Task<Dictionary<string, string>> GetUserInfo()
    {
        var userInfo = new Dictionary<string, string>();
        
        try
        {
            var userId = _currentUserService.UserId;
            if (!string.IsNullOrEmpty(userId))
            {
                userInfo["UserId"] = userId;
                userInfo["UserName"] = await _currentUserService.GetUserNameAsync(userId);
                
                var claims = _httpContextAccessor.HttpContext?.User?.Claims;
                if (claims != null)
                {
                    userInfo["Roles"] = string.Join(",", 
                        claims.Where(c => c.Type == ClaimTypes.Role)
                             .Select(c => c.Value));
                    
                    var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    if (!string.IsNullOrEmpty(email))
                    {
                        userInfo["Email"] = email;
                    }
                }
            }
            else
            {
                userInfo["UserContext"] = "Anonymous";
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Error retrieving user information");
            userInfo["UserContext"] = "Error retrieving user details";
        }

        return userInfo;
    }
}