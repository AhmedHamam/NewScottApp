using FluentValidation;
using Microsoft.AspNetCore.Identity;
using NewScottApp.Domain.Domains.User;

namespace NewScottApp.Application.Commands.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher;
        public ChangePasswordValidator(UserManager<ApplicationUser> userManager
            , PasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;

            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .MustAsync(BeValidUserId);

            RuleFor(x => x.OldPassword)
                .NotNull()
                .NotEmpty()
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{8,15}$")
                .MustAsync(BeValidOldPassword);

            RuleFor(x => x.NewPassword)
                .NotNull()
                .NotEmpty()
                .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{8,15}$");
        }

        private async Task<bool> BeValidUserId(string userId, CancellationToken cancellationToken)
        {
            return await _userManager.FindByIdAsync(userId) != null;
        }

        private async Task<bool> BeValidOldPassword(string oldpassword, CancellationToken cancellationToken)
        {
            string currentuserId = "";
            ApplicationUser user = await _userManager.FindByIdAsync(currentuserId);
            return user.PasswordHash == _passwordHasher.HashPassword(user, oldpassword);
        }
    }
}
