using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Mappers;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync();
        Task<TaskResponseDto?> GetTaskByIdAsync(long id);
        Task<IEnumerable<TaskResponseDto>> GetTasksByAssigneeAsync(long assigneeId);
        Task<IEnumerable<TaskResponseDto>> GetTasksByTeamAsync(long teamId);
        Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto, long userId);
        Task<TaskResponseDto?> UpdateTaskAsync(long id, UpdateTaskDto dto, long userId);
        Task<bool> UpdateTaskStatusAsync(long id, long statusId, long userId);
        Task<bool> DeleteTaskAsync(long id);
    }

    
}