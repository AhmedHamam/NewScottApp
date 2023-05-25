using MediatR;

namespace Configuration.Application.Commands.Countries.CreateCountry
{
    public class CreateCountryCommand : IRequest<int>
    {
        public string CountryNameArabic { get; set; }
        public string CountryNameEnglish { get; set; }
        public int? Code { get; set; }
        public string? Iso { get; set; }
        public string? Nicename { get; set; }
        public string? Iso3 { get; set; }
        public Int16? Numcode { get; set; }
        public int? Phonecode { get; set; }
        public bool IsSaudi { get; set; } = false;
        public int? ExtentionNumber { get; set; }

        public CreateCountryCommand(string countryNameArabic, string countryNameEnglish)
        {
            CountryNameArabic = countryNameArabic;
            CountryNameEnglish = countryNameEnglish;
        }
    }
}
