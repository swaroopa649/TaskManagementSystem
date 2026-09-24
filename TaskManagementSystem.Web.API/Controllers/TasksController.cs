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
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound(new { message = $"Task with ID {id} not found." });

            return Ok(task);
        }

        [HttpGet("assignee/{assigneeId:long}")]
        public async Task<IActionResult> GetByAssignee(long assigneeId)
        {
            var tasks = await _taskService.GetTasksByAssigneeAsync(assigneeId);
            return Ok(tasks);
        }

        [HttpGet("team/{teamId:long}")]
        public async Task<IActionResult> GetByTeam(long teamId)
        {
            var tasks = await _taskService.GetTasksByTeamAsync(teamId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var result = await _taskService.CreateTaskAsync(dto, userId);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateTaskDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetCurrentUserId();
            var result = await _taskService.UpdateTaskAsync(id, dto, userId);
            if (result == null) return NotFound(new { message = $"Task with ID {id} not found." });

            return Ok(result);
        }

        [HttpPatch("{id:long}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateTaskStatusDto dto)
        {
            var userId = GetCurrentUserId();
            var success = await _taskService.UpdateTaskStatusAsync(id, dto.StatusId, userId);
            if (!success) return NotFound(new { message = $"Task with ID {id} not found." });

            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _taskService.DeleteTaskAsync(id);
            if (!success) return NotFound(new { message = $"Task with ID {id} not found." });

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