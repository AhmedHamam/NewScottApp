using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;
using System.Security.Claims;

namespace Configuration.Application.Queries.Countries.GetLoggedUserCountires
{
    public class GetLoggedUserCountires : IRequest<List<LookupDto>>
    {
        public ClaimsIdentity claimsIdentity { get; set; }
        public bool isEnglish { get; set; }

        public GetLoggedUserCountires(ClaimsIdentity claimsIdentity, bool isEnglish)
        {
            this.claimsIdentity = claimsIdentity;
            this.isEnglish = isEnglish;
        }
    }
}
