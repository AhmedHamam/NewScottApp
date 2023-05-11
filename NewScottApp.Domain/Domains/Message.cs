using Base.Domain.CommonModels;

namespace NewScottApp.Domain.Domains
{
    public abstract class Message : BaseEntity<long>
    {
        private string _text;
        private DateTimeOffset _sentTime;

        private Message()
        {

        }
        public Message(string text)
        {
            _text = text;
            _sentTime = DateTime.UtcNow;
        }

        public string Text => _text;
        public DateTimeOffset SentTime => _sentTime;

    }
}
