using Base.API.Controllers;
using Configuration.Api.Helpers;
using Configuration.Application.Queries.Countries.AllCountryWithChilds;
using Configuration.Application.Queries.Countries.AllExtentionNumbetCountries;
using Configuration.Application.Queries.Countries.GetLoggedUserCountires;
using Configuration.Application.Queries.Countries.List;
using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Configuration.Api.Controllers
{
    [ApiController]
    [EnableCors("ApiCorsPolicy")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CountryController : BaseController
    {
        private readonly ILogger<CountryController> _logger;
        public CountryController(ILogger<CountryController> logger, ISender mediator) : base(mediator)
        {
            _logger = logger;
        }

        [Route("List")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CountryDto>))]
        public async Task<ActionResult<IEnumerable<CountryDto>>> ListCountryAsync()
        {
            ListCountries countries = new ListCountries(HeadersHelper.GetLanguageHeader(Request));
            return Ok(await Mediator.Send(countries));
        }


        [HttpGet]
        [Route("ListAll")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CountryDto>))]
        public async Task<ActionResult<IEnumerable<CountryDto>>> ListAllCountryAsync()
        {
            ListCountries countries = new ListCountries(HeadersHelper.GetLanguageHeader(Request));
            return Ok(await Mediator.Send(countries));
        }

        [HttpGet]
        [Route("AllCountryWithChilds")]
        //[Authorize]
        public async Task<ActionResult<List<ParentWithChildsLookup>>> AllCountryWithChilds()
        {
            AllCountryWithChilds countries = new AllCountryWithChilds(HeadersHelper.GetLanguageHeader(Request));
            return Ok(await Mediator.Send(countries));
        }


        [HttpGet]
        [Route("AllExtentionNumbetCountries")]
        public async Task<ActionResult<List<CountryDto>>> AllExtentionNumbetCountries()
        {
            AllExtentionNumberCountries allExtention =
                new AllExtentionNumberCountries();
            return Ok(await Mediator.Send(allExtention));

        }

        [HttpGet]
        [Route("GetLoggedUserCountires")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CountryDto>))]
        [Authorize]
        public async Task<ActionResult<List<ParentWithChildsLookup>>> GetLoggedUserCountires()
        {
            //TODO Need to Implement and test after imlement authorization 
            if (User is not null && User.Identity is not null)
            {
                var claims = User!.Identity! as ClaimsIdentity;
                GetLoggedUserCountires userCountires =
                    new GetLoggedUserCountires(claims!, isEnglish: HeadersHelper.GetLanguageHeader(Request));
                return Ok(await Mediator.Send(userCountires));
            }
            else
            { return BadRequest(); }
        }
    }
}
