using Base.Domain.CommonModels;

namespace Configuration.Domain.Alert
{
    public class AlertConfiguration : AuditableEntity
    {
        public int AlertConfigurationID { get; set; }
        public virtual int AlertClassificationID { get; set; }
        public virtual AlertClassification AlertClassification { get; set; }
        public virtual int AlertProcedureType { get; set; }
        public int? Days { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public string Title { get; set; }

    }
}