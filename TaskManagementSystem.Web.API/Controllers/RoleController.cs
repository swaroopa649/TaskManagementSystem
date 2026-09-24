using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Services;

namespace TaskManagementSystem.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponseDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all roles.");

                var roles = await _roleService.GetAllAsync();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all roles.");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving roles.");
            }
        }

        // GET: api/Role/1
        [HttpGet("{id:long}")]
        public async Task<ActionResult<RoleResponseDto>> GetById(long id)
        {
            try
            {
                _logger.LogInformation("Getting role with Id: {RoleId}", id);

                var role = await _roleService.GetByIdAsync(id);

                if (role == null)
                {
                    _logger.LogWarning(
                        "Role with Id {RoleId} was not found.",
                        id);

                    return NotFound($"Role with Id {id} not found.");
                }

                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while getting role with Id: {RoleId}",
                    id);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving the role.");
            }
        }

        // POST: api/Role
        [HttpPost]
        public async Task<ActionResult<RoleResponseDto>> Create(
            [FromBody] CreateRoleDto dto)
        {
            try
            {
                _logger.LogInformation(
                    "Creating role with Code: {RoleCode}",
                    dto.Code);

                // Replace with your authenticated user ID
                long userId = 1;

                var role = await _roleService.CreateAsync(dto, userId);

                _logger.LogInformation(
                    "Role created successfully with Id: {RoleId}",
                    role.Id);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = role.Id },
                    role);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Unable to create role with Code: {RoleCode}",
                    dto.Code);

                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while creating role.");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the role.");
            }
        }

        // PUT: api/Role/1
        [HttpPut("{id:long}")]
        public async Task<ActionResult<RoleResponseDto>> Update(
            long id,
            [FromBody] UpdateRoleDto dto)
        {
            try
            {
                _logger.LogInformation(
                    "Updating role with Id: {RoleId}",
                    id);

                // Replace with your authenticated user ID
                long userId = 1;

                var role = await _roleService.UpdateAsync(
                    id,
                    dto,
                    userId);

                if (role == null)
                {
                    _logger.LogWarning(
                        "Role with Id {RoleId} was not found.",
                        id);

                    return NotFound($"Role with Id {id} not found.");
                }

                _logger.LogInformation(
                    "Role with Id {RoleId} updated successfully.",
                    id);

                return Ok(role);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Unable to update role with Id: {RoleId}",
                    id);

                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating role with Id: {RoleId}",
                    id);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while updating the role.");
            }
        }

        // PATCH: api/Role/1/status?isActive=false
        [HttpPatch("{id:long}/status")]
        public async Task<IActionResult> UpdateStatus(
            long id,
            [FromQuery] bool isActive)
        {
            try
            {
                _logger.LogInformation(
                    "Updating status for role {RoleId} to {IsActive}.",
                    id,
                    isActive);

                // Replace with your authenticated user ID
                long userId = 1;

                var result = await _roleService.UpdateStatusAsync(
                    id,
                    isActive,
                    userId);

                if (!result)
                {
                    _logger.LogWarning(
                        "Role with Id {RoleId} was not found.",
                        id);

                    return NotFound($"Role with Id {id} not found.");
                }

                _logger.LogInformation(
                    "Role {RoleId} status updated successfully.",
                    id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while updating status for role {RoleId}.",
                    id);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while updating the role status.");
            }
        }

        // DELETE: api/Role/1
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                _logger.LogInformation(
                    "Deleting role with Id: {RoleId}",
                    id);

                // Replace with your authenticated user ID
                long userId = 1;

                var result = await _roleService.DeleteAsync(
                    id,
                    userId);

                if (!result)
                {
                    _logger.LogWarning(
                        "Role with Id {RoleId} was not found.",
                        id);

                    return NotFound($"Role with Id {id} not found.");
                }

                _logger.LogInformation(
                    "Role with Id {RoleId} deleted successfully.",
                    id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while deleting role with Id: {RoleId}",
                    id);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while deleting the role.");
            }
        }
    }
}
