using Base.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertClassificationController : BaseController
    {

        public AlertClassificationController(ISender mediator) : base(mediator)
        {

        }
        //#region Commands
        //[AllowAnonymous]
        //[HttpPost("register")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        //public async Task<ActionResult<string>> RegisterUser(LoginCommand command)
        //{
        //    return Ok(await Mediator.Send(command, CancellationToken));
        //}


        //private readonly ILogger<AlertClassificationController> _logger;

        //private IAlertClassificationRepository _alertClassificationRepository;
        //public AlertClassificationController(IAlertClassificationRepository alertClassificationRepository, ILogger<AlertClassificationController> logger)
        //{
        //    _alertClassificationRepository = alertClassificationRepository;
        //    _logger = logger;
        //}


        [Route("List")]
        [HttpGet]
        public ActionResult List()
        {
            try
            {
                // var result = _alertClassificationRepository.List(HeadersHelper.GetLanguageHeader(Request));
                //if (result == null)
                //    return Problem();
                //return Ok(result);
                var r = Request;
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                //  _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }


    }
}
