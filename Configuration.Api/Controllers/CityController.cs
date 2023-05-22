using Base.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.Api.Controllers
{

    [EnableCors("ApiCorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    internal class CityControlle : BaseController
    {
        public CityControlle(ISender mediator) : base(mediator)
        {
        }
    }
}
