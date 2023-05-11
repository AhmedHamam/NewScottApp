using NewScottApp.Domain.Domains.User;

namespace NewScottApp.Domain.Domains.Groups
{
    public class GroupMessages : Message
    {
        private ApplicationUser _sender;
        private HashSet<GroupMembers> _recivedBy;
        private HashSet<GroupMembers> _seenBy;


        public GroupMessages(ApplicationUser sender, string text) : base(text)
        {
            _sender = sender;
            _seenBy = new HashSet<GroupMembers>();
            _recivedBy = new HashSet<GroupMembers>();
        }

        public ApplicationUser Sender => _sender;
        public HashSet<GroupMembers> RecivedBy => _recivedBy;
        public HashSet<GroupMembers> SeenBy => _seenBy;

    }
}
