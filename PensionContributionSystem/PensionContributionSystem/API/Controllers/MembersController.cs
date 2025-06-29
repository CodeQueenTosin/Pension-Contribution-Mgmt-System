using Microsoft.AspNetCore.Mvc;
using PensionContributionSystem.Application.DTOs;
using PensionContributionSystem.Application.Interfaces;


namespace PensionContributionSystem.API.Controllers
{
    [ApiController]
    [Route("api/v1/members")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _service;

        public MembersController(IMemberService service)
        {
            _service = service;
        }


        /// <summary>
        /// Creates a new member.
        /// </summary>
        /// <param name="dto">The data for the member to create.</param>
        /// <returns>The created member information.</returns>

        [HttpPost]
        public async Task<ActionResult<MemberDto>> Create([FromBody] CreateMemberDto dto)
        {
            var result = await _service.CreateMemberAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a member by their unique identifier.
        /// </summary>
        /// <param name="id">The member ID.</param>
        /// <returns>The member details if found.</returns>

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _service.GetMemberAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Updates an existing member's information.
        /// </summary>
        /// <param name="dto">The data to update the member.</param>
        /// <returns>The updated member information.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateMemberDto dto)
        {
            var updatedMember = await _service.UpdateMemberAsync(dto);
            return Ok(updatedMember);
        }

        /// <summary>
        /// Soft deletes a member by their unique identifier.
        /// </summary>
        /// <param name="id">The member ID.</param>
        /// <returns>A confirmation message if the member was deleted.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the member is not found.</exception>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteMemberAsync(id);
            if (!deleted)
                throw new KeyNotFoundException($"Member with ID {id} not found.");

            return Ok(new { message = $"Member with ID {id} was soft deleted." });
        }


    }

}


