using Microsoft.Extensions.Logging;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Mappers;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(IProjectRepository projectRepository, ILogger<ProjectService> logger)
        {
            _projectRepository = projectRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all projects from service.");
                var projects = await _projectRepository.GetAllProjectsAsync();

                return projects.Select(p => p.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all projects in service.");
                throw;
            }
        }

        public async Task<ProjectResponseDto?> GetByIdProjectAsync(long projectId)
        {
            try
            {
                _logger.LogInformation("Fetching project with ID: {ProjectId}", projectId);

                if (projectId <= 0)
                {
                    _logger.LogWarning("Invalid project ID provided: {ProjectId}", projectId);
                    return null;
                }

                var project = await _projectRepository.GetByIdProjectAsync(projectId);

                if (project == null)
                {
                    _logger.LogWarning("Project with ID {ProjectId} was not found.", projectId);
                    return null;
                }

                return project.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching project with ID: {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto createProjectDto)
        {
            try
            {
                _logger.LogInformation("Creating new project with Key: {ProjectKey}", createProjectDto.ProjectKey);

                var projectEntity = createProjectDto.ToEntity();
                projectEntity.CreatedDate = DateTimeOffset.UtcNow;

                var createdProject = await _projectRepository.AddProjectAsync(projectEntity);

                _logger.LogInformation("Project created successfully with ID: {ProjectId}", createdProject.ProjectId);

                return createdProject.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new project.");
                throw;
            }
        }

        public async Task<bool> UpdateProjectAsync(long projectId, UpdateProjectDto updateProjectDto)
        {
            try
            {
                _logger.LogInformation("Updating project with ID: {ProjectId}", projectId);

                if (projectId <= 0)
                {
                    _logger.LogWarning("Invalid project ID provided for update: {ProjectId}", projectId);
                    return false;
                }

                var existingProject = await _projectRepository.GetByIdProjectAsync(projectId);
                if (existingProject == null)
                {
                    _logger.LogWarning("Project with ID {ProjectId} not found for update.", projectId);
                    return false;
                }

                updateProjectDto.UpdateEntity(existingProject);
                existingProject.ModifiedOn = DateTimeOffset.UtcNow;

                var updated = await _projectRepository.UpdateProjectAsync(projectId, existingProject);

                if (updated == null)
                {
                    return false;
                }

                _logger.LogInformation("Project with ID {ProjectId} updated successfully.", projectId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating project with ID: {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<bool> DeleteProjectAsync(long projectId)
        {
            try
            {
                _logger.LogInformation("Deleting project with ID: {ProjectId}", projectId);

                if (projectId <= 0)
                {
                    _logger.LogWarning("Invalid project ID provided for deletion: {ProjectId}", projectId);
                    return false;
                }

                var success = await _projectRepository.DeleteProjectAsync(projectId);
                if (!success)
                {
                    _logger.LogWarning("Project with ID {ProjectId} was not found for deletion.", projectId);
                    return false;
                }

                _logger.LogInformation("Project with ID {ProjectId} deleted successfully.", projectId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting project with ID: {ProjectId}", projectId);
                throw;
            }
        }
    }
}