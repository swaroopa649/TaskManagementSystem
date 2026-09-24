using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Services;
using TaskManagementSystem.Web.API.Utility;

namespace TaskManagementSystem.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [TMSAuthorizeAttribute]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null) return NotFound(new { message = $"Team with ID {id} not found." });

            return Ok(team);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var result = await _teamService.CreateTeamAsync(dto, userId);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateTeamDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var result = await _teamService.UpdateTeamAsync(id, dto, userId);
            if (result == null) return NotFound(new { message = $"Team with ID {id} not found." });

            return Ok(result);
        }

        [HttpPost("members")]
        public async Task<IActionResult> AssignMember([FromBody] AssignTeamMemberDto dto)
        {
            var userId = GetCurrentUserId();
            var success = await _teamService.AssignMemberAsync(dto, userId);
            if (!success) return BadRequest(new { message = "Member is already added to this team or team does not exist." });

            return Ok(new { message = "Member assigned successfully." });
        }

        [HttpDelete("{teamId:long}/members/{userId:long}")]
        public async Task<IActionResult> RemoveMember(long teamId, long userId)
        {
            var success = await _teamService.RemoveMemberAsync(teamId, userId);
            if (!success) return NotFound(new { message = "Team member assignment not found." });

            return NoContent();
        }

        private long GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            return long.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }
}