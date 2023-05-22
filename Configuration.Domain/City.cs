﻿using Base.Domain.CommonModels;
using NewScotApp.Setup.CurrentUser;

namespace Configuration.Domain
{
    public class City : AuditableEntity
    {
        public City()
        {
        }

        public City(int cityId, string cityNameArabic, string cityNameEnglish)
        {
            CityID = cityId;
            CityNameArabic = cityNameArabic;
            CityNameEnglish = cityNameEnglish;
        }

        public int CityID { get; private set; }
        public string CityNameArabic { get; private set; }
        public string CityNameEnglish { get; private set; }
        public virtual Country? Country { get; private set; }
        public virtual Region? Region { get; private set; }

        public void SetCityNameArabic(string cityNameArabic)
        {
            CityNameArabic = cityNameArabic;
            MarkAsUpdated(CurrentUser.Id);
        }

        public void SetCityNameEnglish(string cityNameEnglish)
        {
            CityNameEnglish = cityNameEnglish;
            MarkAsUpdated(CurrentUser.Id);
        }

        public void SetCountry(Country? country)
        {
            Country = country;
            MarkAsUpdated(CurrentUser.Id);
        }

        public void SetRegion(Region? region)
        {
            Region = region;
            MarkAsUpdated(CurrentUser.Id);
        }
    }
}