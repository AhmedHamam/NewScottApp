using MediatR.Pipeline;
using Serilog;

namespace Base.Application.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    // private readonly ILogger _logger;
    //private readonly ICurrentUserService _currentUserService;
    //private readonly IIdentityService _identityService;

    // public LoggingBehaviour(ILogger<TRequest> logger //ICurrentUserService currentUserService
    //     //IIdentityService identityService
    //     )
    // {
    //     _logger = logger;
    //    // _currentUserService = currentUserService;
    //    // _identityService = identityService;
    // }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = string.Empty; // _currentUserService.UserId ?? string.Empty;
        string userName = string.Empty;

        //if (!string.IsNullOrEmpty(userId))
        //{
        //    userName = await _identityService.GetUserNameAsync(userId);
        //}

        Log.Information("Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
    }
}