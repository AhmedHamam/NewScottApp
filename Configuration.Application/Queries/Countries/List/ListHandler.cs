using Configuration.Infrastructure;
using Configuration.Reprisotry.QueriesRepositories;
using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;

namespace Configuration.Application.Queries.Countries.List
{
    public class ListHandler : IRequestHandler<ListCountries, List<CountryDto>>
    {
        ICountryQueryRepository _repository;
        ConfigurationsDbContext _dbContext;
        public ListHandler(ICountryQueryRepository repository, ConfigurationsDbContext dbContext)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<List<CountryDto>> Handle(ListCountries request, CancellationToken cancellationToken)
        {

            return _repository.List(request.IsEnglish);
            //throw new NotImplementedException();
        }
    }
}
