using Microsoft.AspNetCore.Identity;

namespace NewScottApp.Domain.Domains.User
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public ApplicationUser? User { get; set; }
    }
}
