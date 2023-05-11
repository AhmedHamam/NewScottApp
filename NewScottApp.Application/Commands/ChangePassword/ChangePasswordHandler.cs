using MediatR;
using Microsoft.AspNetCore.Identity;
using NewScottApp.Domain.Domains.User;

namespace NewScottApp.Application.Commands.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher;
        public ChangePasswordHandler(UserManager<ApplicationUser> userManager,
            PasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }
        public async Task<string> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            await _userManager.UpdateAsync(user);
            return "Password Changed";
        }
    }
}
