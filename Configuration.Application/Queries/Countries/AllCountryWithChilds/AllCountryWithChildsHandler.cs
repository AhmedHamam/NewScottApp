using Configuration.Reprisotry.QueriesRepositories;
using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;

namespace Configuration.Application.Queries.Countries.AllCountryWithChilds
{

    public class AllCountryWithChildsHandler : IRequestHandler<AllCountryWithChilds, List<ParentWithChildsLookup>>
    {
        ICountryQueryRepository _repository;
        public AllCountryWithChildsHandler(ICountryQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ParentWithChildsLookup>> Handle(AllCountryWithChilds request, CancellationToken cancellationToken)
        {
            return _repository.GetCountiresWithCities(request.IsEnglish);
        }
    }
}
