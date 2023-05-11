using Base.Domain.CommonModels;

namespace NewScottApp.Domain.Domains
{
    public abstract class Chat : BaseEntity<long>
    {
        List<Message> Messages { get; set; }

    }
}
