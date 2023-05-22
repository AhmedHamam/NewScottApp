namespace Configuration.Reprisotry.QueriesRepositories.Dto
{
    public class ParentWithChildsLookup : LookupDto
    {
        public List<LookupDto> Childs { get; set; }
    }
}