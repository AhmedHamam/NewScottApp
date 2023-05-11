namespace NewScottApp.Domain.Domains.User
{
    public class PrivateMessage : Message
    {
        private ApplicationUser _sender;
        private ApplicationUser _reciver;

        private DateTimeOffset _recivedTime;
        private DateTimeOffset _seenTime;

        public PrivateMessage(string text, ApplicationUser sender, ApplicationUser reciver)
            : base(text)
        {
            _sender = sender;
            _reciver = reciver;
        }
        public ApplicationUser Sender => _sender;
        public ApplicationUser Reciver => _reciver;
        public DateTimeOffset RecivedTime => _recivedTime;
        public DateTimeOffset SeenTime => _seenTime;

        public void SetSeenTime()
        {
            _seenTime = DateTime.UtcNow;
        }

        public void SetRecivedTime()
        {
            _recivedTime = DateTime.UtcNow;
        }


    }
}
