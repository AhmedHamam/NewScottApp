using Base.Domain.CommonModels;

namespace Configuration.Domain.Alert
{
    public class AlertClassification : AuditableEntity
    {
        public AlertClassification()
        {
            AlertConfiguration = new HashSet<AlertConfiguration>();
        }

        public int AlertClassificationID { get; set; }
        public string AlertClassificationArabicName { get; set; }
        public string AlertClassificationEnglishName { get; set; }
        public int? AlertClassificationCode { get; set; }

        public virtual ICollection<AlertConfiguration> AlertConfiguration { get; set; }
    }
}