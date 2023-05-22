using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;

namespace Configuration.Application.Queries.Countries.List
{
    public class ListCountries : IRequest<List<CountryDto>>
    {

        public bool IsEnglish { get; set; }

        public ListCountries(bool isEnglish)
        {
            IsEnglish = isEnglish;
        }
    }
}
