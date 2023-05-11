using Base.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewScottApp.Application.Commands.ChatBoot;

namespace NewScottApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ChatController : BaseController
    {
        public ChatController(ISender mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpPost("chatbot")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<ActionResult<string>> ChatBoot(ChatBot command)
        {
            return Ok(await Mediator.Send(command, CancellationToken));
        }
    }
}
