using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Mappers;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Select(t => t.ToDto());
        }

        public async Task<TaskResponseDto?> GetTaskByIdAsync(long id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task?.ToDto();
        }

        public async Task<IEnumerable<TaskResponseDto>> GetTasksByAssigneeAsync(long assigneeId)
        {
            var tasks = await _taskRepository.GetByAssigneeIdAsync(assigneeId);
            return tasks.Select(t => t.ToDto());
        }

        public async Task<IEnumerable<TaskResponseDto>> GetTasksByTeamAsync(long teamId)
        {
            var tasks = await _taskRepository.GetByTeamIdAsync(teamId);
            return tasks.Select(t => t.ToDto());
        }

        public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto, long userId)
        {
            var entity = dto.ToEntity(userId);
            var created = await _taskRepository.CreateAsync(entity);

            // Reload entity with navigation properties
            var result = await _taskRepository.GetByIdAsync(created.Id);
            return result!.ToDto();
        }

        public async Task<TaskResponseDto?> UpdateTaskAsync(long id, UpdateTaskDto dto, long userId)
        {
            var entity = await _taskRepository.GetByIdAsync(id);
            if (entity == null) return null;

            dto.UpdateEntity(entity, userId);
            await _taskRepository.UpdateAsync(entity);

            var updated = await _taskRepository.GetByIdAsync(id);
            return updated!.ToDto();
        }

        public async Task<bool> UpdateTaskStatusAsync(long id, long statusId, long userId)
        {
            var entity = await _taskRepository.GetByIdAsync(id);
            if (entity == null) return false;

            entity.StatusId = statusId;
            entity.ModifiedBy = userId;
            entity.ModifiedOn = DateTimeOffset.UtcNow;

            await _taskRepository.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteTaskAsync(long id)
        {
            return await _taskRepository.DeleteAsync(id);
        }
    }
}
