using AutoMapper;
using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Domain.Entities;

namespace PensionContributionSystem.Application.Mappings
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<CreateMemberDto, Member>();
            CreateMap<UpdateMemberDto, Member>();
            CreateMap<Member, MemberDto>().ReverseMap();

            CreateMap<ContributionDto, Contribution>();
            CreateMap<Contribution, ContributionDto>();
        }
    }
}
