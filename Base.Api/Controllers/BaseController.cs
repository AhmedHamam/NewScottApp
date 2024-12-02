using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Base.API.Controllers;

/// <summary>
/// Base controller that provides common functionality for all API controllers
/// </summary>
/// <remarks>
/// This controller provides:
/// - MediatR integration for CQRS pattern
/// - Enhanced response handling with HTTP context awareness
/// - Cancellation token support
/// - Safe response methods that check for already started responses
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    private ISender _mediator = null!;

    /// <summary>
    /// Gets or sets the MediatR sender instance for handling commands and queries
    /// </summary>
    protected ISender Mediator
    {
        get => _mediator;
        set => _mediator = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets the HTTP context accessor if the response hasn't started
    /// </summary>
    /// <remarks>
    /// Returns null if the response has already started to prevent
    /// modifications to an already started response
    /// </remarks>
    protected IHttpContextAccessor? Accessor =>
        HttpContext.Response.HasStarted
            ? null
            : HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();

    /// <summary>
    /// Gets the cancellation token for the current request
    /// </summary>
    /// <remarks>
    /// Returns the request's cancellation token if available,
    /// otherwise returns a new default token
    /// </remarks>
    protected CancellationToken CancellationToken
    {
        get
        {
            var httpContext = Accessor?.HttpContext;
            return httpContext?.RequestAborted ?? new CancellationToken();
        }
    }

    /// <summary>
    /// Initializes a new instance of the BaseController
    /// </summary>
    /// <param name="mediator">The MediatR sender instance</param>
    /// <exception cref="ArgumentNullException">Thrown when mediator is null</exception>
    protected BaseController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Creates a NoContent (204) response if the response hasn't started
    /// </summary>
    /// <returns>NoContent result or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult NoContent()
    {
        return Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
                ? new EmptyResult()
                : new NoContentResult();
    }

    /// <summary>
    /// Creates an Ok (200) response if the response hasn't started
    /// </summary>
    /// <returns>Ok result or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult Ok()
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new OkResult();

    /// <summary>
    /// Creates an Ok (200) response with the specified value if the response hasn't started
    /// </summary>
    /// <param name="value">The value to include in the response</param>
    /// <returns>Ok result with value or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult Ok([ActionResultObjectValue] object? value)
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new OkObjectResult(value);

    /// <summary>
    /// Creates an Unauthorized (401) response if the response hasn't started
    /// </summary>
    /// <returns>Unauthorized result or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult Unauthorized()
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new UnauthorizedResult();

    /// <summary>
    /// Creates an Unauthorized (401) response with the specified value if the response hasn't started
    /// </summary>
    /// <param name="value">The value to include in the response</param>
    /// <returns>Unauthorized result with value or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult Unauthorized([ActionResultObjectValue] object? value)
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new UnauthorizedObjectResult(value);

    /// <summary>
    /// Creates a NotFound (404) response if the response hasn't started
    /// </summary>
    /// <returns>NotFound result or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult NotFound()
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new NotFoundResult();

    /// <summary>
    /// Creates a NotFound (404) response with the specified value if the response hasn't started
    /// </summary>
    /// <param name="value">The value to include in the response</param>
    /// <returns>NotFound result with value or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult NotFound([ActionResultObjectValue] object? value)
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new NotFoundObjectResult(value);

    /// <summary>
    /// Creates a BadRequest (400) response if the response hasn't started
    /// </summary>
    /// <returns>BadRequest result or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult BadRequest()
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new BadRequestResult();

    /// <summary>
    /// Creates a BadRequest (400) response with the specified error if the response hasn't started
    /// </summary>
    /// <param name="error">The error details to include in the response</param>
    /// <returns>BadRequest result with error or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult BadRequest([ActionResultObjectValue] object? error)
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new BadRequestObjectResult(error);

    /// <summary>
    /// Creates a Created (201) response with the specified URI and value if the response hasn't started
    /// </summary>
    /// <param name="uri">The URI at which the content has been created</param>
    /// <param name="value">The value to include in the response</param>
    /// <returns>Created result or EmptyResult if response has started</returns>
    /// <exception cref="ArgumentNullException">Thrown when uri is null</exception>
    [NonAction]
    public new ActionResult Created(string uri, [ActionResultObjectValue] object? value)
    {
        ArgumentNullException.ThrowIfNull(uri);

        return Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new CreatedResult(uri, value);
    }

    /// <summary>
    /// Creates a Created (201) response with the specified URI and value if the response hasn't started
    /// </summary>
    /// <param name="uri">The URI at which the content has been created</param>
    /// <param name="value">The value to include in the response</param>
    /// <returns>Created result or EmptyResult if response has started</returns>
    /// <exception cref="ArgumentNullException">Thrown when uri is null</exception>
    [NonAction]
    public new ActionResult Created(Uri uri, [ActionResultObjectValue] object? value)
    {
        ArgumentNullException.ThrowIfNull(uri);

        return Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new CreatedResult(uri, value);
    }

    /// <summary>
    /// Creates a Forbidden (403) response if the response hasn't started
    /// </summary>
    /// <returns>Forbidden result or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult Forbidden()
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new StatusCodeResult(StatusCodes.Status403Forbidden);

    /// <summary>
    /// Creates a Forbidden (403) response with the specified value if the response hasn't started
    /// </summary>
    /// <param name="value">The value to include in the response</param>
    /// <returns>Forbidden result with value or EmptyResult if response has started</returns>
    [NonAction]
    public new ActionResult Forbidden([ActionResultObjectValue] object? value)
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new ObjectResult(value) { StatusCode = StatusCodes.Status403Forbidden };
}