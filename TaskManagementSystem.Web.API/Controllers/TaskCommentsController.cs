using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Services;
using TaskManagementSystem.Web.API.Utility;

namespace TaskManagementSystem.Web.API.Controllers
{
    [ApiController]
    [Route("api/tasks/{taskId:long}/comments")]
    [TMSAuthorizeAttribute]
    public class TaskCommentsController : ControllerBase
    {
        private readonly ITaskCommentService _commentService;

        public TaskCommentsController(ITaskCommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByTaskId(long taskId)
        {
            var comments = await _commentService.GetCommentsByTaskIdAsync(taskId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(long taskId, [FromBody] CreateTaskCommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            dto.TaskId = taskId; // Ensure TaskId matches route parameter
            var userId = GetCurrentUserId();
            var result = await _commentService.AddCommentAsync(dto, userId);

            return Ok(result);
        }

        private long GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;

            return long.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }
}