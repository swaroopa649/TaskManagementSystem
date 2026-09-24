using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Models.Mappers
{
    public static class TaskCommentMapper
    {
        public static TaskCommentResponseDto ToDto(this TaskComment entity)
        {
            return new TaskCommentResponseDto
            {
                Id = entity.Id,
                TaskId = entity.TaskId,
                UserId = entity.UserId,
                UserName = entity.User != null
                    ? $"{entity.User.FirstName} {entity.User.LastName}".Trim()
                    : string.Empty,
                Comment = entity.Comment,
                CreatedOn = entity.CreatedOn
            };
        }

        public static TaskComment ToEntity(this CreateTaskCommentDto dto, long userId)
        {
            return new TaskComment
            {
                TaskId = dto.TaskId,
                UserId = userId,
                Comment = dto.Comment,
                IsActive = true,
                CreatedBy = userId,
                CreatedOn = DateTimeOffset.UtcNow
            };
        }
    }
}