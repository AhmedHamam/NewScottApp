using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewScottApp.Domain.Domains.User
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        [NotMapped]
        public virtual Role Role { get; set; }
    }
}