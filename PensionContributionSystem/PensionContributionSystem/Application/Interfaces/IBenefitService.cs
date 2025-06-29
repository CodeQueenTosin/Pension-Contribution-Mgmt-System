using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Domain.Entities;

namespace PensionContributionSystem.Application.Interfaces
{
    public interface IBenefitService
    {
        Task<BenefitDto> CalculateBenefitsAsync(Guid memberId);
        Task<IEnumerable<Benefit>> GetBenefitsAsync(Guid memberId);
        Task RecalculateAndStoreBenefits();
        Task ApplyMonthlyInterest();

    }
}
