using Base.Infrastructure.Repository;
using Configuration.Domain.Alert;
using Configuration.Reprisotry.QueriesRepositories;
using Configuration.Reprisotry.QueriesRepositories.Dto;

namespace Configuration.Infrastructure.Repositories
{
    public class AlertClassificationRepository : BaseRepository<AlertClassification>, IAlertClassificationRepository
    {
        public AlertClassificationRepository(ConfigurationsDbContext context) : base(context)
        {
        }

        public List<LookupDto> List(bool isEnglish)
        {
            try
            {
                return Find(e => !e.IsDeleted).Select(e => new LookupDto
                {
                    Id = e.AlertClassificationID,
                    Text = isEnglish || string.IsNullOrEmpty(e.AlertClassificationArabicName) ? e.AlertClassificationEnglishName : e.AlertClassificationArabicName,
                    ObjectCode = e.AlertClassificationCode
                }).ToList();
            }
            catch (Exception ex)
            {
                //RepositoryHelper.LogException(ex);
                return null;
            }

        }
    }
}
