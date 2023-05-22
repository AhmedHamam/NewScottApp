using Base.Application.Exceptions;
using Base.Domain.CommonModels;
using NewScotApp.Setup.CurrentUser;

namespace Configuration.Domain
{
    public class Region : AuditableEntity
    {
        private Region()
        {
            City = new HashSet<City>();
        }

        public Region(string regionNameArabic, string regionNameEnglish)
        {
            RegionNameArabic = regionNameArabic;
            RegionNameEnglish = regionNameEnglish;
            City = new HashSet<City>();
        }


        public int RegionID { get; private set; }
        public string RegionNameArabic { get; private set; }
        public string RegionNameEnglish { get; private set; }

        public virtual ICollection<City> City { get; private set; }
        public void AddCity(City city)
        {
            if (city is null)
            {
                throw new ArgumentNullException(nameof(city));
            }
            if (City.Contains(city))
            {
                throw new Exception("City Added before ");
            }
            City.Add(city);
            MarkAsUpdated(CurrentUser.Id);
        }
        public void RemoveCity(City city)
        {
            if (city is null)
            {
                throw new ArgumentNullException(nameof(city));
            }
            if (!City.Contains(city))
            {
                throw new NotFoundException();
            }

            City.Remove(city);
            MarkAsUpdated(CurrentUser.Id);
        }
        public void SetRegionNameArabic(string ArName)
        {
            RegionNameArabic = ArName;
            MarkAsUpdated(CurrentUser.Id);
        }
        public void SetRegionNameEnglish(string EnName)
        {
            RegionNameEnglish = EnName;
            MarkAsUpdated(CurrentUser.Id);
        }

    }
}