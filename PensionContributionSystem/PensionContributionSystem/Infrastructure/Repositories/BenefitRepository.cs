using Microsoft.EntityFrameworkCore;
using PensionContributionSystem.Domain.Entities;
using PensionContributionSystem.Infrastructure.Data;


public class BenefitRepository : IBenefitRepository
{
    private readonly AppDBContext _context;

    public BenefitRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Benefit benefit)
    {
        await _context.Benefits.AddAsync(benefit);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Benefit>> GetByMemberIdAsync(Guid memberId)
    {
        return await _context.Benefits
                             .Where(b => b.MemberId == memberId)
                             .ToListAsync();
    }
    public async Task<IEnumerable<Benefit>> GetAllAsync()
    {
        return await _context.Benefits.ToListAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<Benefit> benefits)
    {
        _context.Benefits.UpdateRange(benefits);
        await _context.SaveChangesAsync();
    }
}
