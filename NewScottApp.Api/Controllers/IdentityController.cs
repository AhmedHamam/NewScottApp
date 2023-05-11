using Base.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using NewScottApp.Application.Commands.ForgetPassword;
using NewScottApp.Application.Commands.Login;

namespace NewScottApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class IdentityController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public IdentityController(ISender mediator, IWebHostEnvironment hostingEnvironment) : base(mediator)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region Commands
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<ActionResult<string>> RegisterUser(LoginCommand command)
        {
            return Ok(await Mediator.Send(command, CancellationToken));
        }


        [AllowAnonymous]
        [HttpPost("forget-password")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<ActionResult<string>> ForgetPassword(ForgetPasswordCommand command)
        {
            return Ok(await Mediator.Send(command, CancellationToken));
        }


        #endregion

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please select a file");

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("File uploaded successfully");
        }
    }
}