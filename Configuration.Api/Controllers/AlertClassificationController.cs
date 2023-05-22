using Base.API.Controllers;
using Configuration.Api.Helpers;
using Configuration.Application.Queries.ListNotification;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.Api.Controllers
{
    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class AlertClassificationController : BaseController
    {



        public AlertClassificationController(ISender mediator)
            : base(mediator)
        {

        }


        [Route("List")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Unit))]
        public ActionResult List()
        {
            try
            {
                ListNotification notification = new ListNotification();
                notification.isEnglish = HeadersHelper.GetLanguageHeader(Request);
                return Ok(Mediator.Send(notification));
                //throw new NotImplementedException();

            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message);
                return Problem(ex.Message);
            }
        }


    }
}
