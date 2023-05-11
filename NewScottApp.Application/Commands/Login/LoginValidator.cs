using FluentValidation;
using Microsoft.AspNetCore.Identity;
using NewScottApp.Domain.Domains.User;

namespace NewScottApp.Application.Commands.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        private UserManager<ApplicationUser> _userManager;
        public LoginValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            CascadeMode = CascadeMode.Stop;
            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(20)
                .MustAsync(UniqueUserName).WithMessage("UserName already exists");


            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .MustAsync(UniqueEmail).WithMessage("Email already exists");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(20)
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain 1 lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain non alphanumeric");

            RuleFor(x => x.FirstName)
               .Cascade(CascadeMode.Stop)
               .NotNull()
               .NotEmpty()
               .MinimumLength(2)
               .MaximumLength(20);

            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(20);
        }

        private async Task<bool> UniqueEmail(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }
        private async Task<bool> UniqueUserName(string userName, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user != null;
        }
    }
}
