using Microsoft.AspNetCore.Identity;

namespace NewScottApp.Domain.Domains.User
{
    public enum Gender
    {
        male = 1,
        female = 2
    }
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
        public string? ConnectionId { get; set; }
        public string? LastSeen { get; set; }


        private readonly HashSet<UserRole> _userRoles = new();
        private readonly HashSet<UserClaim> _claims = new();
        private readonly HashSet<UserLogin> _logins = new();
        private readonly HashSet<UserToken> _tokens = new();
        private readonly HashSet<UserActivity> _activities = new();
        //full parameter constructor

        public ApplicationUser(string firstName, string lastName, DateTime birthDate, Gender gender)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            Avatar = "default.png";
            Status = "offline";
            ConnectionId = null!;
            LastSeen = null!;
        }

        // default constructor

        public ApplicationUser()
        {
            Avatar = "default.png";
            Status = "offline";
            ConnectionId = null!;
            LastSeen = null!;
        }


        public HashSet<UserRole> UserRoles { get { return _userRoles; } }
        public HashSet<UserClaim> Claims { get { return _claims; } }
        public HashSet<UserActivity> UserActivities { get { return _activities; } }
        public HashSet<UserLogin> UserLogins { get { return _logins; } }
        public HashSet<UserToken> UserTokens { get { return _tokens; } }


    }
}
