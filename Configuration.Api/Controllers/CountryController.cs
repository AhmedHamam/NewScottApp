using Base.API.Controllers;
using Configuration.Api.Helpers;
using Configuration.Application.Queries.Countries.List;
using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Configuration.Api.Controllers
{
    [ApiController]
    [EnableCors("ApiCorsPolicy")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CountryController : BaseController
    {
        private readonly ILogger<CountryController> _logger;
        public CountryController(ISender mediator) : base(mediator)
        {

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
        [Authorize]
        public ActionResult<List<ParentWithChildsLookup>> AllCountryWithChilds()
        {

            throw new NotImplementedException();
        }


        //[HttpGet]
        //[Route("AllExtentionNumbetCountries")]
        //public ActionResult<List<CountryViewModel>> AllExtentionNumbetCountries()
        //{
        //    try
        //    {
        //        var response = new RepositoryOutput();
        //        var result = _countryRepository.ExtentionNumberCountries();
        //        if (result == null)
        //        {
        //            if (response.Code == RepositoryResponseStatus.Error || !response.Success)
        //                return Problem();
        //        }
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return Problem(ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("GetLoggedUserCountires")]
        //[Authorize]
        //public ActionResult<List<ParentWithChildsLookup>> GetLoggedUserCountires()
        //{
        //    try
        //    {
        //        var response = new RepositoryOutput();
        //        var result = _countryRepository.ListLoggedUserCountries(User.Identity as ClaimsIdentity, HeadersHelper.GetLanguageHeader(Request));
        //        if (result == null)
        //        {
        //            if (response.Code == RepositoryResponseStatus.Error || !response.Success)
        //                return Problem();
        //        }

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return Problem(ex.Message);
        //    }
        //}
    }
}
