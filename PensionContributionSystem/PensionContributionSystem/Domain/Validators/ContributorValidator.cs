using FluentValidation;
using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Domain.Entities;

namespace PensionContributionSystem.Domain.Validators
{
    public class ContributionDtoValidator : AbstractValidator<ContributionDto>
    {
        public ContributionDtoValidator()
        {
            RuleFor(x => x.ContributionType)
                  .NotEmpty().WithMessage("Contribution type is required.")
                  .Must(type => type == "Monthly" || type == "Voluntary")
                  .WithMessage("Contribution type must be either 'Monthly' or 'Voluntary'.");

            RuleFor(x => x.Amount).GreaterThan(0);
         
            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage("Reference number is required.")
                .Matches("^[a-zA-Z0-9-]{5,20}$")
                .WithMessage("Reference number must be 5–20 characters, letters, numbers, or hyphens.");

            RuleFor(x => x.ContributionDate)
                         .NotEmpty().WithMessage("Contribution date is required.");
            RuleFor(x => x).Custom((contrib, context) => {
                if (contrib.ContributionType == "Monthly")
                {
                }
            });
        }
    }
}
