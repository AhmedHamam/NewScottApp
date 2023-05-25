using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;

namespace Configuration.Application.Queries.Countries.AllExtentionNumbetCountries
{
    public class AllExtentionNumberCountries : IRequest<List<CountryDto>>
    {
    }
}
