using Configuration.Reprisotry.QueriesRepositories;
using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;

namespace Configuration.Application.Queries.Countries.GetLoggedUserCountires
{
    public class GetLoggedUserCountiresHandler : IRequestHandler<GetLoggedUserCountires, List<LookupDto>>
    {
        ICountryQueryRepository _repository;
        public GetLoggedUserCountiresHandler(ICountryQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LookupDto>> Handle(GetLoggedUserCountires request, CancellationToken cancellationToken)
        {
            return _repository.ListLoggedUserCountries(request.claimsIdentity, request.isEnglish);
        }

    }
}
