using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project?> GetByIdProjectAsync(long projectId);
        Task<Project> AddProjectAsync(Project project);
        Task<Project?> UpdateProjectAsync(long projectId, Project project);
        Task<bool> DeleteProjectAsync(long projectId);
    }
}
