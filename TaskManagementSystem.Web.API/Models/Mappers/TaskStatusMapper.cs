using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Models.Mappers
{
    public static class TaskStatusMapper
    {
        public static TaskStatusResponseDto ToDto(this TaskManagementSystem.Web.API.Models.Entities.TaskStatus entity)
        {
            return new TaskStatusResponseDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                IsActive = entity.IsActive
            };
        }

        public static TaskManagementSystem.Web.API.Models.Entities.TaskStatus ToEntity(this CreateTaskStatusDto dto)
        {
            return new TaskManagementSystem.Web.API.Models.Entities.TaskStatus
            {
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                IsActive = dto.IsActive,
                CreatedOn = DateTimeOffset.UtcNow
            };
        }
    }
}