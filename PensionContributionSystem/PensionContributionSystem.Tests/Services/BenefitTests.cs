using Moq;
using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using PensionContributionSystem.Infrastructure.Repositories;

namespace PensionContributionSystem.Tests.Services
{
    public class BenefitServiceTests
    {
        private readonly Mock<IContributionRepository> _contributionRepoMock = new();
        private readonly Mock<IMemberRepository> _memberRepoMock = new();
        private readonly Mock<IBenefitRepository> _benefitRepoMock = new();
        private readonly Mock<IValidator<ContributionDto>> _validatorMock = new();
        private readonly Mock<ILogger<BenefitService>> _loggerMock = new();

        private readonly BenefitService _service;

        public BenefitServiceTests()
        {
            _service = new BenefitService(
                _contributionRepoMock.Object,
                _memberRepoMock.Object,
                _benefitRepoMock.Object,
                _validatorMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CalculateBenefitsAsync_ValidContributions_ReturnsBenefitDto_AndStoresBenefit()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var contributions = new List<Contribution>
            {
                new Contribution { MemberId = memberId, Amount = 1000, ContributionDate = DateTime.UtcNow.AddMonths(-13) },
                new Contribution { MemberId = memberId, Amount = 2000, ContributionDate = DateTime.UtcNow.AddMonths(-12) }
            };

            _contributionRepoMock.Setup(r => r.GetByMemberIdAsync(memberId))
                .ReturnsAsync(contributions);

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ContributionDto>(), default))
                .ReturnsAsync(new ValidationResult());

            _benefitRepoMock.Setup(r => r.AddAsync(It.IsAny<Benefit>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CalculateBenefitsAsync(memberId);

            // Assert
            Assert.True(result.Eligible);
            Assert.Equal(memberId, result.MemberId);
            Assert.Equal(300, result.BenefitAmount); // 10% of 3000

            _benefitRepoMock.Verify(r => r.AddAsync(It.IsAny<Benefit>()), Times.Once);
        }

        [Fact]
        public async Task CalculateBenefitsAsync_NoContributions_ThrowsException()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            _contributionRepoMock.Setup(r => r.GetByMemberIdAsync(memberId)).ReturnsAsync(new List<Contribution>());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CalculateBenefitsAsync(memberId));
        }

        [Fact]
        public async Task CalculateBenefitsAsync_LessThan12Months_ThrowsException()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var contributions = new List<Contribution>
            {
                new Contribution { MemberId = memberId, Amount = 1000, ContributionDate = DateTime.UtcNow.AddMonths(-5) }
            };

            _contributionRepoMock.Setup(r => r.GetByMemberIdAsync(memberId))
                .ReturnsAsync(contributions);

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ContributionDto>(), default))
                .ReturnsAsync(new ValidationResult());

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CalculateBenefitsAsync(memberId));
        }

        [Fact]
        public async Task GetBenefitsAsync_ReturnsBenefitsForMember()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var member = new Member { Id = memberId };
            var benefits = new List<Benefit>
            {
                new Benefit { MemberId = memberId, Amount = 100 },
                new Benefit { MemberId = memberId, Amount = 200 }
            };

            _memberRepoMock.Setup(r => r.GetByIdAsync(memberId)).ReturnsAsync(member);
            _benefitRepoMock.Setup(r => r.GetByMemberIdAsync(memberId)).ReturnsAsync(benefits);

            // Act
            var result = await _service.GetBenefitsAsync(memberId);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetBenefitsAsync_MemberNotFound_ThrowsKeyNotFound()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            _memberRepoMock.Setup(r => r.GetByIdAsync(memberId)).ReturnsAsync((Member)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetBenefitsAsync(memberId));
        }
    }
}
