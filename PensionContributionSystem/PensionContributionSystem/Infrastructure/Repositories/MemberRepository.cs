using Microsoft.EntityFrameworkCore;
using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Domain.Entities;
using PensionContributionSystem.Infrastructure.Data;


namespace PensionContributionSystem.Infrastructure.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDBContext _context;

        public MemberRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Member member)
        {
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        public async Task<Member> GetByIdAsync(Guid id)
        {
            return await _context.Members.FindAsync(id);
        }

        public async Task<List<Member>> GetAllAsync()
        {
            return await _context.Members.ToListAsync();
        }

        public async Task UpdateAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                member.IsDeleted = true; 
                await _context.SaveChangesAsync();
            }
        }
    }
}