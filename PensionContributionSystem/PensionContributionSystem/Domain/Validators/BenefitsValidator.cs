using FluentValidation;
using PensionContributionSystem.Domain.Entities;

public class BenefitValidator : AbstractValidator<Benefit>
{
    public BenefitValidator()
    {
        RuleFor(x => x.BenefitType)
            .NotEmpty()
            .WithMessage("Benefit type is required.");

        RuleFor(x => x.CalculationDate)
            .NotEmpty()
            .WithMessage("Calculation date is required.")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Calculation date cannot be in the future.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Benefit amount must be greater than zero.");
    }
}
