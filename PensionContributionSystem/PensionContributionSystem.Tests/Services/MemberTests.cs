using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Application.Services;
using PensionContributionSystem.Domain.Entities;
using PensionContributionSystem.Infrastructure.Repositories;
using AutoMapper;

namespace PensionContributionSystem.Tests.Services
{
    public class MemberServiceTests
    {
        private readonly Mock<IMemberRepository> _repoMock = new();
        private readonly Mock<IValidator<CreateMemberDto>> _createValidatorMock = new();
        private readonly Mock<IValidator<UpdateMemberDto>> _updateValidatorMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private readonly MemberService _service;

        public MemberServiceTests()
        {
            _service = new MemberService(
                _repoMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object,
                 _mapperMock.Object
            );
        }

        [Fact]
        public async Task CreateMemberAsync_ValidDto_ReturnsMemberDto()
        {
            var dto = new CreateMemberDto
            {
                FirstName = "Tosin",
                LastName = "Olorunnisola",
                Email = "tosin@example.com",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            _createValidatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Member>())).Returns(Task.CompletedTask);

            var result = await _service.CreateMemberAsync(dto);

            Assert.Equal("Tosin", result.FirstName);
            Assert.Equal("Olorunnisola", result.LastName);
        }

        [Fact]
        public async Task UpdateMemberAsync_MemberNotFound_ThrowsKeyNotFoundException()
        {
            var dto = new UpdateMemberDto
            {
                MemberId = Guid.NewGuid(),
                FirstName = "Updated"
            };

            _repoMock.Setup(r => r.GetByIdAsync(dto.MemberId)).ReturnsAsync((Member)null!);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateMemberAsync(dto));
        }

        [Fact]
        public async Task GetMemberAsync_ReturnsMemberDto()
        {
            var id = Guid.NewGuid();
            var member = new Member { Id = id, FirstName = "Tosin" };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(member);

            var result = await _service.GetMemberAsync(id);

            Assert.Equal("Tosin", result.FirstName);
        }

        [Fact]
        public async Task DeleteMemberAsync_SoftDeletesMember_ReturnsTrue()
        {
            var id = Guid.NewGuid();
            var member = new Member { Id = id };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(member);
            _repoMock.Setup(r => r.UpdateAsync(member)).Returns(Task.CompletedTask);

            var result = await _service.DeleteMemberAsync(id);

            Assert.True(result);
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Member>(m => m.IsDeleted)), Times.Once);
        }
    }
}
