using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public interface ITaskCommentRepository
    {
        Task<IEnumerable<TaskComment>> GetByTaskIdAsync(long taskId);
        Task<TaskComment> CreateAsync(TaskComment entity);
    }
}
