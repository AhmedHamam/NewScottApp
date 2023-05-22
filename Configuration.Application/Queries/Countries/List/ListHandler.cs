using Configuration.Reprisotry.QueriesRepositories;
using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;

namespace Configuration.Application.Queries.Countries.List
{
    public class ListHandler : IRequestHandler<ListCountries, List<CountryDto>>
    {
        ICountryQueryRepository _repository;

        public ListHandler(ICountryQueryRepository repository)
        {
            _repository = repository;
        }

        public Task<List<CountryDto>> Handle(ListCountries request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_repository.List(request.IsEnglish));
        }
    }
}
