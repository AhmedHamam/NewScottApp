using Base.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Collections.Generic;

namespace NewScottApp.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GroupChatController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public GroupChatController(ISender mediator, IWebHostEnvironment hostingEnvironment) : base(mediator)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        //[HttpPost("upload")]
        //public async Task<IActionResult> Upload(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file selected");

        //    // generate unique identifier
        //    var fileId = Guid.NewGuid().ToString();

        //    // get file extension
        //    var fileExtension = Path.GetExtension(file.FileName);

        //    // create new file name with unique id and original extension
        //    var newFileName = fileId + fileExtension;

        //    // save file to disk with new name
        //    var filePath = Path.Combine(_uploadPath, newFileName);
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return Ok("File uploaded successfully.");
        //}
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm]
        IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file selected");

            // generate unique identifier
            var fileId = Guid.NewGuid().ToString();

            // get file extension
            var fileExtension = Path.GetExtension(file.FileName);

            // create new file name with unique id and original extension
            var newFileName = fileId + fileExtension;

            // save file to disk with new name
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads", newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("File uploaded successfully\n" + fileId);
        }
        //upload list of files at the same time 

        [HttpPost("upload-list")]
        public async Task<IActionResult> Upload([FromForm] IFormFile[] files)
        {
            if (files == null || files.Length == 0)
                return BadRequest("Please select at least one file");
            string fields = "";
            foreach (var file in files)
            {
                var fileId = Guid.NewGuid().ToString();

                // get file extension
                var fileExtension = Path.GetExtension(file.FileName);

                // create new file name with unique id and original extension
                var newFileName = fileId + fileExtension;
                fields += fileId + "\n";
                // save file to disk with new name
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "uploads", newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok("File uploaded successfully\n" + fields);
        }
    }
}
