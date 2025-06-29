using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Domain.Entities;

namespace PensionContributionSystem.Infrastructure.Repositories
{
    public interface IMemberRepository
    {
        Task<Member> GetByIdAsync(Guid id);
        Task<List<Member>> GetAllAsync();
        Task AddAsync(Member member);
        Task UpdateAsync(Member member);
        Task SoftDeleteAsync(Guid id);
    }
}