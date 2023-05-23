using Base.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.Api.Controllers
{

    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : BaseController
    {
        public CityController(ISender mediator) : base(mediator)
        {
        }
    }
}
