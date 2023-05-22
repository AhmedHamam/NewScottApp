using Base.Infrastructure.Repository;
using Configuration.Domain;
using Configuration.Reprisotry.QueriesRepositories.Dto;
using System.Security.Claims;

namespace Configuration.Reprisotry.QueriesRepositories
{
    public interface ICountryQueryRepository : IBaseRepository<Country>
    {
        List<CountryDto> List(bool isEnglish);
        public List<ParentWithChildsLookup> GetCountiresWithCities(bool isEnglish);
        public List<CountryDto> ExtentionNumberCountries();
        List<CountryDto> ListAll(bool isEnglish);
        List<LookupDto> ListLoggedUserCountries(ClaimsIdentity userClaims, bool isEnglish);
    }
}
