using Configuration.Reprisotry.QueriesRepositories.Dto;
using MediatR;

namespace Configuration.Application.Queries.Countries.AllCountryWithChilds
{
    public class AllCountryWithChilds : IRequest<List<ParentWithChildsLookup>>
    {

        public bool IsEnglish { get; set; }

        public AllCountryWithChilds(bool isEnglish)
        {
            IsEnglish = isEnglish;
        }
    }
}
