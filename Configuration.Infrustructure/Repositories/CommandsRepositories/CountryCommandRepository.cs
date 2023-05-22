using Base.Infrastructure.Persistence;
using Base.Infrastructure.Repository;
using Configuration.Domain;
using Configuration.Reprisotry.CommandsRepositories;

namespace Configuration.Infrastructure.Repositories.CommandsRepositories
{
    public class CountryCommandRepository : BaseRepository<Country>, ICountryCommandRepository
    {
        public CountryCommandRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
