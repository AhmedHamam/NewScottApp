using Configuration.Reprisotry.QueriesRepositories;
using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;

namespace Configuration.Application.Queries.Countries.AllExtentionNumbetCountries
{
    public class AllExtentionNumbetCountriesHandler : IRequestHandler<AllExtentionNumberCountries, List<CountryDto>>
    {
        ICountryQueryRepository _repository;
        public AllExtentionNumbetCountriesHandler(ICountryQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CountryDto>> Handle(AllExtentionNumberCountries request, CancellationToken cancellationToken)
        {
            return _repository.ExtentionNumberCountries();
        }
    }
}
