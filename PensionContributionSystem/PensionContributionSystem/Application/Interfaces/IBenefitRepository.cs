using PensionContributionSystem.Domain.Entities;

public interface IBenefitRepository
{
    Task AddAsync(Benefit benefit);
    Task<IEnumerable<Benefit>> GetByMemberIdAsync(Guid memberId);
    Task<IEnumerable<Benefit>> GetAllAsync();
    Task UpdateRangeAsync(IEnumerable<Benefit> benefits);
}