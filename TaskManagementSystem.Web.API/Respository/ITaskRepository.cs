using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TeamTask>> GetAllAsync();
        Task<TeamTask?> GetByIdAsync(long id);
        Task<IEnumerable<TeamTask>> GetByAssigneeIdAsync(long assigneeId);
        Task<IEnumerable<TeamTask>> GetByTeamIdAsync(long teamId);
        Task<TeamTask> CreateAsync(TeamTask entity);
        Task<TeamTask> UpdateAsync(TeamTask entity);
        Task<bool> DeleteAsync(long id);
    }
}
