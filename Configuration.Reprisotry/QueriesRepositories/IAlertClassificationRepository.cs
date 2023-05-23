using Configuration.Reprisotry.QueriesRepositories.Dto;


namespace Configuration.Reprisotry.QueriesRepositories
{

    public interface IAlertClassificationRepository
    {
        List<LookupDto> List(bool isEnglish);
    }
}
