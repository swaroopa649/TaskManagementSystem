using TaskManagementSystem.Web.API.Models.DTOs;

namespace TaskManagementSystem.Web.API.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync();
        Task<ProjectResponseDto?> GetByIdProjectAsync(long projectId);
        Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto createProjectDto);
        Task<bool> UpdateProjectAsync(long projectId, UpdateProjectDto updateProjectDto);
        Task<bool> DeleteProjectAsync(long projectId);
    }
}