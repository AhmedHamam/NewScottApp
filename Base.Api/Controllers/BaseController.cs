using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Base.API.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    private ISender _mediator = null!;

    public ISender Mediator
    {
        get
        {
            return _mediator;
        }
        protected set
        {
            _mediator = value;
        }
    }


    protected IHttpContextAccessor? Accessor =>
        HttpContext.Response.HasStarted
            ? null
            : HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();


    protected CancellationToken CancellationToken
    {
        get
        {
            var httpContext = Accessor?.HttpContext;
            return httpContext?.RequestAborted ?? new CancellationToken();
        }
    }

    public BaseController(ISender mediator)
    {
        _mediator = mediator;
    }

    [NonAction]
    public new ActionResult NoContent()
    {
        return Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
                ? new EmptyResult()
                : new NoContentResult();
    }

    [NonAction]
    public new ActionResult Ok()
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new OkResult();

    [NonAction]
    public new ActionResult Ok([ActionResultObjectValue] object? value)
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new OkObjectResult(value);

    [NonAction]
    public new ActionResult Unauthorized()
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new UnauthorizedResult();

    [NonAction]
    public new ActionResult Unauthorized([ActionResultObjectValue] object? value)
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new UnauthorizedObjectResult(value);

    [NonAction]
    public new ActionResult NotFound()
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new NotFoundResult();

    [NonAction]
    public new ActionResult NotFound([ActionResultObjectValue] object? value)
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new NotFoundObjectResult(value);

    [NonAction]
    public new ActionResult BadRequest()
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new BadRequestResult();

    [NonAction]
    public new ActionResult BadRequest([ActionResultObjectValue] object? error)
        => Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new BadRequestObjectResult(error);

    [NonAction]
    public new ActionResult Created(string uri, [ActionResultObjectValue] object? value)
    {
        if (uri == null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        return Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new CreatedResult(uri, value);
    }

    [NonAction]
    public new ActionResult Created(Uri uri, [ActionResultObjectValue] object? value)
    {
        if (uri == null)
        {
            throw new ArgumentNullException(nameof(uri));
        }

        return Accessor?.HttpContext?.Response == null || Accessor.HttpContext.Response.HasStarted
            ? new EmptyResult()
            : new CreatedResult(uri, value);
    }
}