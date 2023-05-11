using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewScottApp.Domain.Domains.User
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public int Id { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        [NotMapped]
        public int UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}