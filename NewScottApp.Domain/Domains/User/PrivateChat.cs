namespace NewScottApp.Domain.Domains.User
{
    public class PrivateChat : Chat
    {
        private ApplicationUser _user1;
        private ApplicationUser _user2;
        private List<PrivateMessage> _messages;

        public PrivateChat(ApplicationUser user1, ApplicationUser user2)
        {
            _user1 = user1;
            _user2 = user2;
            _messages = new List<PrivateMessage>();
        }

        public ApplicationUser User1 => _user1;
        public ApplicationUser User2 => _user2;
        public List<PrivateMessage> Messages => _messages;

        public void AddMessage(PrivateMessage message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            _messages.Add(message);
        }

        public void RemoveMessage(long userId, PrivateMessage message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            message.MarkAsDeleted(userId);
            _messages.Remove(message);
        }


    }
}
