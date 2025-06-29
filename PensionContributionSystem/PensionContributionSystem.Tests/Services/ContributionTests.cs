using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Domain.Entities;
using PensionContributionSystem.Infrastructure.Repositories;
using static PensionContributionSystem.Domain.Enum.ContributorTypeEnum;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace PensionContributionSystem.Tests.Services
{
    public class ContributionServiceTests
    {
        private readonly Mock<IContributionRepository> _repoMock = new();
        private readonly Mock<IMemberRepository> _memberRepoMock = new();
        private readonly Mock<IValidator<ContributionDto>> _validatorMock = new();
        private readonly Mock<IMapper> _mapperMock = new(); 
        private readonly Mock<ILogger<ContributionService>> _loggerMock = new(); 

        private readonly ContributionService _service;

        public ContributionServiceTests()
        {
            _service = new ContributionService(
                _repoMock.Object,
                _memberRepoMock.Object,
                _validatorMock.Object,
                _mapperMock.Object,   
                _loggerMock.Object 
            );
        }
        

        [Fact]
        public async Task AddContributionAsync_ValidDto_AddsContribution()
        {
            var dto = new ContributionDto
            {
                MemberId = Guid.NewGuid(),
                Amount = 500,
                ContributionDate = DateTime.UtcNow
            };

            _validatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult());

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Contribution>()))
                .Returns(Task.CompletedTask);

            await _service.AddContributionAsync(dto);

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Contribution>()), Times.Once);
        }

        [Fact]
        public async Task AddContributionAsync_InvalidDto_ThrowsValidationException()
        {
            var dto = new ContributionDto { MemberId = Guid.NewGuid() };

            _validatorMock.Setup(v => v.ValidateAsync(dto, default))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                    new("Amount", "Amount is required")
                }));

            await Assert.ThrowsAsync<ValidationException>(() => _service.AddContributionAsync(dto));
        }

        [Fact]
        public async Task GetMemberContributionsAsync_ReturnsContributions()
        {
            var memberId = Guid.NewGuid();
            var list = new List<Contribution>
            {
                new() { Amount = 100 },
                new() { Amount = 200 }
            };

            _repoMock.Setup(r => r.GetByMemberIdAsync(memberId)).ReturnsAsync(list);

            var result = await _service.GetMemberContributionsAsync(memberId);

            Assert.Equal(2, result.Count());
        }


        [Fact]
        public async Task CalculateTotalContributionsAsync_ReturnsCorrectTotal()
        {
            var memberId = Guid.NewGuid();
            var list = new List<Contribution>
            {
                new() { Amount = 100, ContributionType = ContributionType.Monthly.ToString()},
                new() { Amount = 200, ContributionType = ContributionType.Monthly.ToString()}
            };

            _repoMock.Setup(r => r.GetByMemberIdAsync(memberId)).ReturnsAsync(list);

            var total = await _service.CalculateTotalContributionsAsync(memberId, ContributionType.Monthly);

            Assert.Equal(300, total);
        }
    }
}
