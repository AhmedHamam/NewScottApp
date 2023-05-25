using Base.Application.Exceptions;
using Base.Domain.CommonModels;
using NewScotApp.Setup.CurrentUser;

namespace Configuration.Domain
{

    public class Country : AuditableEntity
    {
        private Country()
        {
            City = new HashSet<City>();
        }

        public Country(string countryNameArabic, string countryNameEnglish)
        {
            CountryNameArabic = countryNameArabic;
            CountryNameEnglish = countryNameEnglish;
            City = new HashSet<City>();
        }


        public int CountryID { get; private set; }
        public string? CountryNameArabic { get; private set; }
        public string? CountryNameEnglish { get; private set; }
        public int? Code { get; private set; }
        public string? Iso { get; private set; }
        public string? Nicename { get; private set; }
        public string? Iso3 { get; private set; }
        public int? Numcode { get; private set; }
        public int? Phonecode { get; private set; }
        public bool IsSaudi { get; private set; } = false;
        public int? ExtentionNumber { get; private set; }
        public virtual ICollection<City> City { get; set; }

        public void SetCountryNameArabic(string countryNameArabic)
        {
            CountryNameArabic = countryNameArabic;
            MarkAsUpdated(CurrentUser.Id);
        }

        public void SetCountryNameEnglish(string countryNameEnglish)
        {
            CountryNameEnglish = countryNameEnglish;
            MarkAsUpdated(CurrentUser.Id);

        }
        public void SetCode(int? code)
        {
            Code = code;
            MarkAsUpdated(CurrentUser.Id);
        }
        public void SetIso(string? iso)
        {
            Iso = iso;
            MarkAsUpdated(CurrentUser.Id);
        }
        public void SetNicename(string? nicename)
        {
            Nicename = nicename;
            MarkAsUpdated(CurrentUser.Id);
        }
        public void SetIso3(string? iso3)
        {
            Iso3 = iso3;
            MarkAsUpdated(CurrentUser.Id);
        }
        public void SetNumcode(int? numcode)
        {
            Numcode = numcode;
            MarkAsUpdated(CurrentUser.Id);
        }

        public void SetPhonecode(int? phonecode)
        {
            Phonecode = phonecode;
            MarkAsUpdated(CurrentUser.Id);
        }
        public void SetIsSaudi(bool isSaudi)
        {
            IsSaudi = isSaudi;
            MarkAsUpdated(CurrentUser.Id);
        }

        public void SetExtentionNumber(int? extentionNumber)
        {
            ExtentionNumber = extentionNumber;
            MarkAsUpdated(CurrentUser.Id);
        }
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
    }


}
