using AutoMapper;
using FluentValidation;
using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Application.Interfaces;
using PensionContributionSystem.Domain.Entities;
using PensionContributionSystem.Infrastructure.Repositories;

namespace PensionContributionSystem.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;
        private readonly IValidator<CreateMemberDto> _createValidator;
        private readonly IValidator<UpdateMemberDto> _updateValidator;
        private readonly IMapper _mapper;

        public MemberService(IMemberRepository repository, IValidator<CreateMemberDto> createvalidator, IValidator<UpdateMemberDto> updatevalidator, IMapper mapper)
        {
            _repository = repository;
            _createValidator = createvalidator;
            _updateValidator = updatevalidator;
            _mapper = mapper;
        }

        public async Task<MemberDto> CreateMemberAsync(CreateMemberDto dto)
        {
            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var member = _mapper.Map<Member>(dto);
            await _repository.AddAsync(member);

            return _mapper.Map<MemberDto>(member);
        }

        public async Task<MemberDto> GetMemberAsync(Guid id)
        {
            var member = await _repository.GetByIdAsync(id);

            if (member == null || member.IsDeleted)
                throw new KeyNotFoundException($"Member with ID {id} not found.");

            return _mapper.Map<MemberDto>(member);
        }

        public async Task<MemberDto> UpdateMemberAsync(UpdateMemberDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var member = await _repository.GetByIdAsync(dto.MemberId);
            if (member == null || member.IsDeleted)
                throw new KeyNotFoundException($"Member with ID {dto.MemberId} not found.");

            _mapper.Map(dto, member);
            await _repository.UpdateAsync(member);

            return _mapper.Map<MemberDto>(member);
        }

        public async Task<bool> DeleteMemberAsync(Guid id)
        {
            var member = await _repository.GetByIdAsync(id);
            if (member == null || member.IsDeleted)
                throw new KeyNotFoundException($"Member with ID {id} not found.");

            member.IsDeleted = true;
            await _repository.UpdateAsync(member);

            return true;
        }

    }
}


