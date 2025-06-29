using PensionContributionSystem.Application.DTOs;
namespace PensionContributionSystem.Application.Interfaces
{
    public interface IMemberService
    {
            Task<MemberDto> CreateMemberAsync(CreateMemberDto dto);
            Task<MemberDto> UpdateMemberAsync(UpdateMemberDto dto);
            Task<MemberDto> GetMemberAsync(Guid id);
            Task<bool> DeleteMemberAsync(Guid id);
    }
}
