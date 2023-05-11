using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewScottApp.Domain.Domains.User
{
    public class UserRole : IdentityUserRole<int>
    {
        [NotMapped]
        public virtual Role Role { get; set; }
        [NotMapped]
        public virtual ApplicationUser User { get; set; }
    }
}