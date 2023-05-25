using FluentValidation;

namespace Configuration.Application.Commands.Countries.CreateCountry
{
    public class CreateCountryValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryValidator()
        {
        }

    }
}
