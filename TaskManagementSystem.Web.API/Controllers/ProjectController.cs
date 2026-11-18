using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Services;
using TaskManagementSystem.Web.API.Utility;

namespace TaskManagementSystem.Web.API.Controllers
{
    [ApiController]
    [TMSAuthorizeAttribute]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetAllProjects()
        {
            try
            {
                _logger.LogInformation("Getting all projects.");

                var projects = await _projectService.GetAllProjectsAsync();

                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all projects.");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving projects.");
            }
        }

        [HttpGet("{projectId:long}")]
        public async Task<ActionResult<ProjectResponseDto>> GetProjectById(long projectId)
        {
            try
            {
                if (projectId <= 0)
                {
                    return BadRequest("Invalid project ID.");
                }

                _logger.LogInformation("Getting project with Id: {ProjectId}", projectId);

                var project = await _projectService.GetByIdProjectAsync(projectId);

                if (project == null)
                {
                    _logger.LogWarning("Project with Id {ProjectId} was not found.", projectId);

                    return NotFound($"Project with Id {projectId} not found.");
                }

                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting project with Id: {ProjectId}", projectId);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving the project.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponseDto>> CreateProject([FromBody] CreateProjectDto createProjectDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating project with Key: {ProjectKey}", createProjectDto.ProjectKey);

                var createdProject = await _projectService.CreateProjectAsync(createProjectDto);

                _logger.LogInformation("Project created successfully with Id: {ProjectId}", createdProject.ProjectId);

                return CreatedAtAction(
                    nameof(GetProjectById),
                    new { projectId = createdProject.ProjectId },
                    createdProject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating project.");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the project.");
            }
        }

        [HttpPut("{projectId:long}")]
        public async Task<IActionResult> UpdateProject(long projectId, [FromBody] UpdateProjectDto updateProjectDto)
        {
            try
            {
                if (projectId <= 0)
                {
                    return BadRequest("Invalid project ID.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Updating project with Id: {ProjectId}", projectId);

                var success = await _projectService.UpdateProjectAsync(projectId, updateProjectDto);

                if (!success)
                {
                    _logger.LogWarning("Project with Id {ProjectId} was not found for update.", projectId);

                    return NotFound($"Project with Id {projectId} not found.");
                }

                _logger.LogInformation("Project with Id {ProjectId} updated successfully.", projectId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating project with Id: {ProjectId}", projectId);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while updating the project.");
            }
        }

        [HttpDelete("{projectId:long}")]
        public async Task<IActionResult> DeleteProject(long projectId)
        {
            try
            {
                if (projectId <= 0)
                {
                    return BadRequest("Invalid project ID.");
                }

                _logger.LogInformation("Deleting project with Id: {ProjectId}", projectId);

                var success = await _projectService.DeleteProjectAsync(projectId);

                if (!success)
                {
                    _logger.LogWarning("Project with Id {ProjectId} was not found for deletion.", projectId);

                    return NotFound($"Project with Id {projectId} not found.");
                }

                _logger.LogInformation("Project with Id {ProjectId} deleted successfully.", projectId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting project with Id: {ProjectId}", projectId);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while deleting the project.");
            }
        }
    }
}