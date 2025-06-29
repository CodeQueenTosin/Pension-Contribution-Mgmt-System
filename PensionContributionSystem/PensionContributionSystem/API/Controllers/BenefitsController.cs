using Microsoft.AspNetCore.Mvc;
using PensionContributionSystem.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class BenefitsController : ControllerBase
{
    private readonly IBenefitService _service;

    public BenefitsController(IBenefitService service)
    {
        _service = service;
    }

    /// <summary>
    /// Calculates and stores benefit for a specific member.
    /// </summary>
    [HttpPost("calculate/{memberId}")]
    public async Task<IActionResult> Calculate(Guid memberId)
    {
        await _service.CalculateBenefitsAsync(memberId);
        return Ok(new { message = "Benefit calculated and stored." });
    }

    /// <summary>
    /// Gets all benefits for a member.
    /// </summary>
    [HttpGet("{memberId}")]
    public async Task<IActionResult> Get(Guid memberId)
    {
        var result = await _service.GetBenefitsAsync(memberId);
        return Ok(result);
    }

}
