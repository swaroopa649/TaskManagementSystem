using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Web.API.Data;
using TaskManagementSystem.Web.API.Models.Entities;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Respository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<ProjectRepository> _logger;

        public ProjectRepository(ApplicationDBContext context, ILogger<ProjectRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            try
            {
                return await _context.Set<Project>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching all projects.");
                throw;
            }
        }

        public async Task<Project?> GetByIdProjectAsync(long projectId)
        {
            try
            {
                return await _context.Set<Project>().FindAsync(projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching project with ID {ProjectId}.", projectId);
                throw;
            }
        }

        public async Task<Project> AddProjectAsync(Project project)
        {
            try
            {
                project.CreatedDate = DateTimeOffset.UtcNow;
                _context.Set<Project>().Add(project);
                await _context.SaveChangesAsync();
                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error occurred while adding a new project.");
                throw;
            }
        }

        public async Task<Project?> UpdateProjectAsync(long projectId, Project project)
        {
            try
            {
                var existingProject = await _context.Set<Project>().FindAsync(projectId);
                if (existingProject == null)
                {
                    return null;
                }

                existingProject.ProjectKey = project.ProjectKey;
                existingProject.Name = project.Name;
                existingProject.Description = project.Description;
                existingProject.LeadUserId = project.LeadUserId;
                existingProject.ModifiedBy = project.ModifiedBy;
                existingProject.ModifiedOn = project.ModifiedOn;
                existingProject.IsActive = project.IsActive;

                await _context.SaveChangesAsync();
                return existingProject;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error occurred while updating project with ID {ProjectId}.", projectId);
                throw;
            }
        }

        public async Task<bool> DeleteProjectAsync(long projectId)
        {
            try
            {
                var project = await _context.Set<Project>().FindAsync(projectId);
                if (project == null)
                {
                    return false;
                }

                _context.Set<Project>().Remove(project);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error occurred while deleting project with ID {ProjectId}.", projectId);
                throw;
            }
        }
    }
}