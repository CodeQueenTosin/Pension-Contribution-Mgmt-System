using FluentValidation;
using PensionContributionSystem.Application.DTOs;

namespace PensionContributionSystem.Application.Validators
{
    public class CreateMemberDtoValidator : AbstractValidator<CreateMemberDto>
    {
        public CreateMemberDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is not valid.");
            RuleFor(x => x.DateOfBirth)
                .Must(dob => AgeBetween(dob, 18, 70))
                .WithMessage("Age must be between 18 and 70 years");

            RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^(?:\+234|0)[789][01]\d{8}$").WithMessage("Phone number must be a valid Nigerian number.");

        }

        private bool AgeBetween(DateTime dob, int min, int max)
        {
            var age = DateTime.Today.Year - dob.Year;
            if (dob > DateTime.Today.AddYears(-age)) age--;
            return age >= min && age <= max;
        }
    }
}
