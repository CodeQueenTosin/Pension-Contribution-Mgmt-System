using Microsoft.AspNetCore.Mvc;
using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Application.Interfaces;
using static PensionContributionSystem.Domain.Enum.ContributorTypeEnum;

[ApiController]
[Route("api/v1/contributions")]
public class ContributionsController : ControllerBase
{
    private readonly IContributionService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContributionsController"/> class.
    /// </summary>
    /// <param name="service">The contribution service.</param>

    public ContributionsController(IContributionService service) => _service = service;


    /// <summary>
    /// Adds a new contribution for a member.
    /// </summary>
    /// <param name="dto">The contribution data transfer object.</param>
    /// <returns>A response indicating the contribution was recorded.</returns>
    [HttpPost]
    public async Task<IActionResult> AddContribution([FromBody] ContributionDto dto)
    {
        await _service.AddContributionAsync(dto);
        return Created("", new { message = "Contribution recorded successfully." });
    }

    /// <summary>
    /// Retrieves all contributions for a specific member.
    /// </summary>
    /// <param name="memberId">The unique identifier of the member.</param>
    /// <returns>A list of contributions for the member.</returns>

    [HttpGet]
    public async Task<IActionResult> GetMemberContributions([FromQuery] Guid memberId)
    {
        var result = await _service.GetMemberContributionsAsync(memberId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the total contribution amount for a member based on contribution type.
    /// </summary>
    /// <param name="memberId">The unique identifier of the member.</param>
    /// <param name="type">The contribution type (e.g., Voluntary, Monthly).</param>
    /// <returns>The total contribution for the specified type.</returns>
    [HttpGet("total/{memberId}/{type}")]
    public async Task<IActionResult> GetTotalByType(Guid memberId, ContributionType type)
    {
        var total = await _service.CalculateTotalContributionsAsync(memberId, type);

        return Ok(new
        {
            message = $"Your total contribution for type {type} is {total:N2}.",
            total
        });
    }

    /// <summary>
    /// Retrieves a detailed contribution statement for a specific member.
    /// </summary>
    /// <param name="memberId">The unique identifier of the member.</param>
    /// <returns>A detailed statement of contributions.</returns>
    [HttpGet("{memberId}/statement")]
    public async Task<IActionResult> GetStatement(Guid memberId)
    {
        var statement = await _service.GetContributionStatementAsync(memberId);
        return Ok(statement);
    }

}