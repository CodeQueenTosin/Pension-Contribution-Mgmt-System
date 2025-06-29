using FluentValidation;
using PensionContributionSystem.Domain.Entities;

public class EmployerDtoValidator : AbstractValidator<Employer>
{
    public EmployerDtoValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("Company name is required.");

        RuleFor(x => x.RegistrationNumber)
            .NotEmpty()
            .WithMessage("Registration number is required.")
            .Matches(@"^[A-Z0-9\-]{6,20}$")
            .WithMessage("Registration number must be 6–20 characters long, using uppercase letters, numbers, or dashes.");

        RuleFor(x => x.IsActive)
            .Equal(true)
            .WithMessage("Employer must be active.");
    }
}
