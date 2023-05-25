using Configuration.Domain;
using Configuration.Reprisotry.CommandsRepositories;
using MediatR;

namespace Configuration.Application.Commands.Countries.CreateCountry
{
    public class CreateCountryHandler : IRequestHandler<CreateCountryCommand, int>
    {
        ICountryCommandRepository _repository;

        public CreateCountryHandler(ICountryCommandRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var country = new Country(request.CountryNameArabic, request.CountryNameEnglish);
            country.SetNumcode(request.Numcode);
            country.SetPhonecode(request.Phonecode);
            country.SetCode(request.Code);
            country.SetExtentionNumber(request.ExtentionNumber);
            country.SetIso(request.Iso);
            country.SetIsSaudi(request.IsSaudi);
            country.SetIso3(request.Iso3);
            country.SetNicename(request.Nicename);
            await _repository.AddAsync(country);
            return country.CountryID;
        }
    }
}
