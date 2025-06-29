using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Domain.Entities;
using static PensionContributionSystem.Domain.Enum.ContributorTypeEnum;

namespace PensionContributionSystem.Infrastructure.Repositories
{
    public interface IContributionRepository
    {
        Task AddAsync(Contribution contribution);
        Task<List<Contribution>> GetByMemberIdAsync(Guid memberId);
        Task<Contribution?> GetByMemberIdAndMonthAsync(Guid memberId, int year, int month, ContributionType type);

        Task<IEnumerable<Contribution>> GetMemberContributionsAsync(Guid memberId);
        Task<decimal> CalculateTotalContributionsAsync(Guid memberId, ContributionType type);

    }
}