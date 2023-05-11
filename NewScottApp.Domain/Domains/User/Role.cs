using Microsoft.AspNetCore.Identity;

namespace NewScottApp.Domain.Domains.User
{
    public class Role : IdentityRole<int>
    {
        private string _displayName;
        private readonly ICollection<RoleClaim> _roleClaims;

        public Role(string name, string displayName)
        {
            Name = name;
            _displayName = displayName;
            NormalizedName = name.ToUpper();
        }

        public string DisplayName
        {
            get => _displayName;
            private set => _displayName = value;
        }

        public virtual ICollection<RoleClaim> RoleClaims => _roleClaims;
    }
}