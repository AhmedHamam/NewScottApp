using Base.Domain.CommonModels;
using NewScottApp.Domain.Domains.User;

namespace NewScottApp.Domain.Domains.Groups
{
    public class GroupMembers
    {
        private readonly ApplicationUser _user;
        private readonly DateTime _joinDate;
        private readonly DateTime _leaveDate;
        public GroupMembers(ApplicationUser user)
        {
            _user = user;
        }
        public DateTime JoinDate => _joinDate;
        public DateTime LeaveDate => _leaveDate;
        public ApplicationUser User => _user;
    }
}
