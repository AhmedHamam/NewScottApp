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
            try
            {
                return Find().Select(e => new CountryDto
                {
                    Id = e.CountryID,
                    Text = e.Phonecode.ToString()
                }).OrderBy(e => e.Text).ToList();
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        public List<ParentWithChildsLookup> GetCountiresWithCities(bool isEnglish)
        {
            try
            {

                return Find(null!, null!, "City").Select(p => new ParentWithChildsLookup
                {
                    Id = p.CountryID,
                    Text = isEnglish || string.IsNullOrEmpty(p.CountryNameArabic) ? p.CountryNameEnglish! : p.CountryNameArabic!,
                    Childs = p.City.Select(c => new LookupDto
                    {
                        Id = c.CityID,
                        Text = isEnglish || string.IsNullOrEmpty(c.CityNameArabic) ? c.CityNameEnglish : c.CityNameArabic,
                    }).ToList()
                }).ToList();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CountryDto> List(bool isEnglish)
        {
            try
            {
                return Find(e => !e.IsSaudi).Select(e => new CountryDto
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
            //try
            //{
            //    var userSession = _authenticateRepository.LoadUserSession(userClaims);
            //    var query = Find(e => !e.IsDeleted);
            //    switch (userSession.UserType)
            //    {

            //        case ScotAppInfrastructureDAL.Enums.UserTypeEnum.ScotUser:
            //            if (userSession.CityIds != null && userSession.CityIds.Any())
            //                query = query.Where(e => e.City != null && e.City.Any(c => !c.IsDeleted && userSession.CityIds.Contains(c.CityID)));

            //            break;
            //            //case ScotAppInfrastructureDAL.Enums.UserTypeEnum.HospitalUser:
            //            //    break;
            //            //case ScotAppInfrastructureDAL.Enums.UserTypeEnum.SRCA_ReferalCenterUser:
            //            //    break;
            //    }
            //    return query.Select(e => new LookupViewModel
            //    {
            //        Id = e.CountryID,
            //        Text = isEnglish || string.IsNullOrEmpty(e.CountryNameArabic) ? e.CountryNameEnglish : e.CountryNameArabic
            //    }).ToList(); ;
            //}
            //catch (Exception ex)
            //{
            //    //RepositoryHelper.LogException(ex);
            //    return null;
            //}
            throw new NotImplementedException();
        }
    }
}
