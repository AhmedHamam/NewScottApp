using Base.Infrastructure.Repository;
using Configuration.Domain;
using Configuration.Reprisotry.QueriesRepositories;
using Configuration.Reprisotry.QueriesRepositories.Dto;
using System.Security.Claims;

namespace Configuration.Infrastructure.Repositories.QueriesRepositories
{
    public class CountryQueryRepository : BaseRepository<Country>, ICountryQueryRepository
    {
        public CountryQueryRepository(ConfigurationsDbContext context) : base(context)
        {
        }

        public List<CountryDto> ExtentionNumberCountries()
        {
            throw new NotImplementedException();
        }

        public List<ParentWithChildsLookup> GetCountiresWithCities(bool isEnglish)
        {
            throw new NotImplementedException();
        }

        public List<CountryDto> List(bool isEnglish)
        {
            try
            {
                return Find(e => !e.IsDeleted && !e.IsSaudi).Select(e => new CountryDto
                {
                    Id = e.CountryID,
                    Text = isEnglish || string.IsNullOrEmpty(e.CountryNameArabic) ? e.CountryNameEnglish : e.CountryNameArabic,
                    Extension = e.Phonecode.ToString()
                }).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<CountryDto> ListAll(bool isEnglish)
        {
            try
            {
                return Find(e => !e.IsDeleted).Select(e => new CountryDto
                {
                    Id = e.CountryID,
                    Text = isEnglish || string.IsNullOrEmpty(e.CountryNameArabic) ? e.CountryNameEnglish : e.CountryNameArabic,
                    Extension = e.Phonecode.ToString()
                }).ToList();
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<LookupDto> ListLoggedUserCountries(ClaimsIdentity userClaims, bool isEnglish)
        {
            throw new NotImplementedException();
        }
    }
}
