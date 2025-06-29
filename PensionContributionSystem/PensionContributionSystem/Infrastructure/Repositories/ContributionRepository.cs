using PensionContributionSystem.Application.Interfaces;
using PensionContributionSystem.Domain.Entities;
using PensionContributionSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static PensionContributionSystem.Domain.Enum.ContributorTypeEnum;

namespace PensionContributionSystem.Infrastructure.Repositories
{
    public class ContributionRepository : IContributionRepository
    {
        private readonly AppDBContext _context;

        public ContributionRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Contribution contribution)
        {
            await _context.Contributions.AddAsync(contribution);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Contribution>> GetByMemberIdAsync(Guid memberId)
        {
            return await _context.Contributions
                .Where(c => c.MemberId == memberId)
                .ToListAsync();
        }

        public async Task<Contribution?> GetByMemberIdAndMonthAsync(Guid memberId, int year, int month, ContributionType type)
        {
            return await _context.Contributions
                .FirstOrDefaultAsync(c =>
                    c.MemberId == memberId &&
                    c.ContributionType == type.ToString() &&
                    c.ContributionDate.Year == year &&
                    c.ContributionDate.Month == month);
        }

        public async Task<IEnumerable<Contribution>> GetMemberContributionsAsync(Guid memberId)
        {
            return await _context.Contributions
                .Where(c => c.MemberId == memberId)
                .ToListAsync();
        }

        public async Task<decimal> CalculateTotalContributionsAsync(Guid memberId, ContributionType type)
        {
            return await _context.Contributions
                .Where(c => c.MemberId == memberId && c.ContributionType == type.ToString())
                .SumAsync(c => c.Amount);
        }
    }

}

