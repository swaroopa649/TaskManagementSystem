using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Models.Mappers
{
    public static class TaskMapper
    {
        public static TaskResponseDto ToDto(this TaskManagementSystem.Web.API.Models.Entities.TeamTask entity)
        {
            return new TaskResponseDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                StatusId = entity.StatusId,
                StatusName = entity.Status?.Name ?? string.Empty,
                Priority = entity.Priority,
                TeamId = entity.TeamId,
                TeamName = entity.Team?.Name,
                AssigneeId = entity.AssigneeId,
                AssigneeName = entity.Assignee != null
                    ? $"{entity.Assignee.FirstName} {entity.Assignee.LastName}".Trim()
                    : null,
                DueDate = entity.DueDate,
                IsActive = entity.IsActive,
                CreatedBy = entity.CreatedBy,
                CreatedByName = entity.Creator != null
                    ? $"{entity.Creator.FirstName} {entity.Creator.LastName}".Trim()
                    : null,
                CreatedOn = entity.CreatedOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                Comments = entity.Comments?.Select(c => c.ToDto()).ToList() ?? new List<TaskCommentResponseDto>()
            };
        }

        public static TaskManagementSystem.Web.API.Models.Entities.TeamTask ToEntity(this CreateTaskDto dto, long createdByUserId)
        {
            return new TaskManagementSystem.Web.API.Models.Entities.TeamTask
            {
                Title = dto.Title,
                Description = dto.Description,
                StatusId = dto.StatusId,
                Priority = dto.Priority,
                TeamId = dto.TeamId,
                AssigneeId = dto.AssigneeId,
                DueDate = dto.DueDate,
                IsActive = true,
                CreatedBy = createdByUserId,
                CreatedOn = DateTimeOffset.UtcNow
            };
        }

        public static void UpdateEntity(this UpdateTaskDto dto, TaskManagementSystem.Web.API.Models.Entities.TeamTask entity, long modifiedByUserId)
        {
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.StatusId = dto.StatusId;
            entity.Priority = dto.Priority;
            entity.TeamId = dto.TeamId;
            entity.AssigneeId = dto.AssigneeId;
            entity.DueDate = dto.DueDate;
            entity.IsActive = dto.IsActive;
            entity.ModifiedBy = modifiedByUserId;
            entity.ModifiedOn = DateTimeOffset.UtcNow;
        }
    }
}