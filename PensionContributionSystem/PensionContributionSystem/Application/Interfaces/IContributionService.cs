using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Domain.Entities;
using static PensionContributionSystem.Domain.Enum.ContributorTypeEnum;

namespace PensionContributionSystem.Application.Interfaces
{
    public interface IContributionService
    {
        Task<string> AddContributionAsync(ContributionDto dto);
        Task<IEnumerable<ContributionDto>> GetMemberContributionsAsync(Guid memberId);
        Task<decimal> CalculateTotalContributionsAsync(Guid memberId, ContributionType type);
        Task<ContributionStatementDto> GetContributionStatementAsync(Guid memberId);
        Task ValidateMonthlyContributions();
        Task GenerateMemberStatements();
    }
}
