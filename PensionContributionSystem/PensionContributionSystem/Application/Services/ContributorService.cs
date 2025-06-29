using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Application.Interfaces;
using PensionContributionSystem.Domain.Entities;
using PensionContributionSystem.Infrastructure.Repositories;
using AutoMapper;
using FluentValidation;

using static PensionContributionSystem.Domain.Enum.ContributorTypeEnum;

public class ContributionService : IContributionService
{
    private readonly IContributionRepository _contributionRepo;
    private readonly IMemberRepository _memberRepo;
    private readonly IMapper _mapper;
    private readonly IValidator<ContributionDto> _validator;
    private readonly ILogger<ContributionService> _logger;

    public ContributionService(
        IContributionRepository contributionRepo,
        IMemberRepository memberRepo,
        IValidator<ContributionDto> validator,
        IMapper mapper,
        ILogger<ContributionService> logger)
    {
        _contributionRepo = contributionRepo;
        _memberRepo = memberRepo;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<string> AddContributionAsync(ContributionDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var member = await _memberRepo.GetByIdAsync(dto.MemberId);
        if (member == null)
            throw new KeyNotFoundException("Member not found");

        if (dto.ContributionType == ContributionType.Monthly.ToString())
        {
            var existing = await _contributionRepo.GetByMemberIdAndMonthAsync(
                dto.MemberId,
                dto.ContributionDate.Year,
                dto.ContributionDate.Month,
                ContributionType.Monthly
            );

            if (existing != null)
                throw new InvalidOperationException("Monthly contribution has been made this month");
        }

        var contribution = _mapper.Map<Contribution>(dto);

        await _contributionRepo.AddAsync(contribution);
        return "Contribution recorded successfully.";
    }

    private decimal CalculateInterest(decimal amount, DateTime date)
    {
        // Simple monthly interest calculation (e.g. 1%)
        return amount * 0.01m;
    }

    public async Task<ContributionStatementDto> GetContributionStatementAsync(Guid memberId)
    {
        var contributions = await _contributionRepo.GetByMemberIdAsync(memberId);

        if (contributions == null || !contributions.Any())
            throw new KeyNotFoundException("Member not found or has no contributions.");

        var mandatoryTotal = contributions
            .Where(c => c.ContributionType == ContributionType.Monthly.ToString())
            .Sum(c => c.Amount);

        var voluntaryTotal = contributions
            .Where(c => c.ContributionType == ContributionType.Voluntary.ToString())
            .Sum(c => c.Amount);

        return new ContributionStatementDto
        {
            MemberId = memberId,
            MandatoryTotal = mandatoryTotal,
            VoluntaryTotal = voluntaryTotal,
            Contributions = contributions
                .Select(c => _mapper.Map<ContributionDto>(c))
                .ToList()
        };
    }


    public async Task ValidateMonthlyContributions()
    {
        var allMembers = await _memberRepo.GetAllAsync();
        foreach (var member in allMembers)
        {
            var exists = await _contributionRepo.GetByMemberIdAndMonthAsync(
                member.Id, DateTime.UtcNow.Year, DateTime.UtcNow.Month, ContributionType.Monthly);
            if (exists == null)
            {
                _logger.LogWarning("Missing monthly mandatory contribution for Member {MemberId}", member.Id);
                //Run Email Notifification
            }
        }

        Console.WriteLine($"[Hangfire] Running contribution validation at {DateTime.Now}");
    }



    public async Task GenerateMemberStatements()
    {
        var allMembers = await _memberRepo.GetAllAsync();
        foreach (var member in allMembers)
        {
            try
            {
                var statement = await GetContributionStatementAsync(member.Id);
                _logger.LogInformation("Generated contribution statement for Member {MemberId}", member.Id);
                //persist statements or send them via email
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate statement for Member {MemberId}", member.Id);
            }
        }
        Console.WriteLine($"[Hangfire] Generating MemeberStatements at {DateTime.Now}");
    }


    public async Task<IEnumerable<ContributionDto>> GetMemberContributionsAsync(Guid memberId)
    {
        var contributions = await _contributionRepo.GetMemberContributionsAsync(memberId);

        if (contributions == null || !contributions.Any())
            throw new KeyNotFoundException("Member not found or has no contributions.");

        return contributions.Select(c => _mapper.Map<ContributionDto>(c));
    }


    public async Task<decimal> CalculateTotalContributionsAsync(Guid memberId, ContributionType type)
    {
        var total = await _contributionRepo.CalculateTotalContributionsAsync(memberId, type);

        if (total == 0)
            throw new KeyNotFoundException("Member not found or has no contributions of the specified type.");

        return total;
    }

}
