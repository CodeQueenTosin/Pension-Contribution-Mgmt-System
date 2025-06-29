using FluentValidation;
using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Application.Exceptions;
using PensionContributionSystem.Application.Interfaces;
using PensionContributionSystem.Domain.Entities;
using PensionContributionSystem.Infrastructure.Repositories;

public class BenefitService : IBenefitService
{
    private readonly IContributionRepository _contributionRepo;
    private readonly IMemberRepository _memberRepo;
    private readonly IBenefitRepository _benefitRepo;
    private readonly IValidator<ContributionDto> _validator;
    private readonly ILogger<BenefitService> _logger;

    public BenefitService(
        IContributionRepository contributionRepo,
        IMemberRepository memberRepo,
        IBenefitRepository benefitRepo,
        IValidator<ContributionDto> validator,
        ILogger<BenefitService> logger)
    {
        _contributionRepo = contributionRepo;
        _memberRepo = memberRepo;
        _benefitRepo = benefitRepo;
        _validator = validator;
        _logger = logger;
    }


    public async Task<IEnumerable<Benefit>> GetBenefitsAsync(Guid memberId)
    {
        var member = await _memberRepo.GetByIdAsync(memberId);
        if (member == null)
            throw new KeyNotFoundException($"Member with ID '{memberId}' was not found.");

        return await _benefitRepo.GetByMemberIdAsync(memberId);
    }
    public async Task<BenefitDto> CalculateBenefitsAsync(Guid memberId)
    {
        var contributions = await _contributionRepo.GetByMemberIdAsync(memberId);

        if (!contributions.Any())
            throw new InvalidOperationException("No contributions found.");

        var startDate = contributions.Min(c => c.ContributionDate);
        var months = ((DateTime.UtcNow.Year - startDate.Year) * 12) + DateTime.UtcNow.Month - startDate.Month;

        if (months < 12)
            throw new BusinessRuleException("Member must contribute for at least 12 months to be eligible for benefits.");


        var totalAmount = contributions.Sum(c => c.Amount);
        var benefitAmount = totalAmount * 0.1m;

        var benefit = new Benefit
        {
            Id = Guid.NewGuid(),
            MemberId = memberId,
            Amount = benefitAmount,
            BenefitType = "Calculated",
            CalculationDate = DateTime.UtcNow,
            EligibilityStatus = true
        };

        await _benefitRepo.AddAsync(benefit);

        return new BenefitDto
        {
            MemberId = memberId,
            TotalContribution = totalAmount,
            BenefitAmount = benefitAmount,
            Eligible = true
        };
    }


    public async Task RecalculateAndStoreBenefits()
    {
        var allMembers = await _memberRepo.GetAllAsync();

        foreach (var member in allMembers)
        {
            try
            {
                var benefit = await CalculateBenefitsAsync(member.Id);

                var benefitEntity = new Benefit
                {
                    Id = Guid.NewGuid(),
                    MemberId = benefit.MemberId,
                    Amount = benefit.BenefitAmount,
                    BenefitType = "Recalculated",
                    CalculationDate = DateTime.UtcNow,
                    EligibilityStatus = benefit.Eligible
                };

                await _benefitRepo.AddAsync(benefitEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to recalculate benefits for Member {MemberId}", member.Id);
            }
        }
    }

    public async Task ApplyMonthlyInterest()
    {
        var benefits = await _benefitRepo.GetAllAsync();

        foreach (var benefit in benefits)
        {
            benefit.Amount += benefit.Amount * 0.01m; 
        }

        await _benefitRepo.UpdateRangeAsync(benefits);
    }

}
