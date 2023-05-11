using MediatR;
using Microsoft.AspNetCore.Identity;
using NewScottApp.Domain.Domains.User;
using NewScottApp.Infrastructure;

namespace NewScottApp.Application.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        IdentityDatabaseContext _context;
        UserManager<ApplicationUser> _userManager;

        public LoginHandler(UserManager<ApplicationUser> userManager, IdentityDatabaseContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //Your business
            var user = new ApplicationUser();
            user.UserName = request.UserName;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            user.EmailConfirmed = false;
            user.PhoneNumberConfirmed = false;
            user.LockoutEnabled = false;

            try
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded) throw new Exception(result.Errors.ToStringDetails());

            }
            catch (Exception ex)
            {

                return ex.Message + Environment.NewLine + "Inner" + ex.InnerException?.Message;
            }

            return user.Id.ToString();
        }

    }
}
