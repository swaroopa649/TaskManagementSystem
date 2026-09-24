using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Services;
using TaskManagementSystem.Web.API.Utility;

namespace TaskManagementSystem.Web.API.Controllers
{
    [ApiController]
    [TMSAuthorizeAttribute]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all users.");

                var users = await _userService.GetAllAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting users.");

                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }

        // GET: api/User/1
        [HttpGet("{id:long}")]
        public async Task<ActionResult<UserResponseDto>> GetById(long id)
        {
            try
            {
                _logger.LogInformation("Getting user with Id: {UserId}", id);

                var user = await _userService.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound($"User with Id {id} not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user {UserId}.", id);

                return StatusCode(500, "An error occurred while retrieving the user.");
            }
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create([FromBody] CreateUserDto dto)
        {
            try
            {
                _logger.LogInformation("Creating user with email: {Email}", dto.Email);

                // Replace with authenticated user ID
                long userId = 1;

                var user = await _userService.CreateAsync(dto, userId);

                _logger.LogInformation("User created successfully. UserId: {UserId}", user.Id);

                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Unable to create user.");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user.");

                return StatusCode(500, "An error occurred while creating the user.");
            }
        }

        // PUT: api/User/1
        [HttpPut("{id:long}")]
        public async Task<ActionResult<UserResponseDto>> Update(long id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                _logger.LogInformation("Updating user with Id: {UserId}", id);

                // Replace with authenticated user ID
                long userId = 1;

                var user = await _userService.UpdateAsync(id, dto, userId);

                if (user == null)
                {
                    return NotFound($"User with Id {id} not found.");
                }

                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Unable to update user {UserId}.", id);

                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user {UserId}.", id);

                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        // PATCH: api/User/1/status?isActive=false
        [HttpPatch("{id:long}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromQuery] bool isActive)
        {
            try
            {
                _logger.LogInformation("Updating status for user {UserId} to {IsActive}.", id, isActive);

                // Replace with authenticated user ID
                long userId = 1;

                var result = await _userService.UpdateStatusAsync(id, isActive, userId);

                if (!result)
                {
                    return NotFound($"User with Id {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for user {UserId}.", id);

                return StatusCode(500, "An error occurred while updating user status.");
            }
        }

        // DELETE: api/User/1
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                _logger.LogInformation("Deleting user with Id: {UserId}", id);

                // Replace with authenticated user ID
                long userId = 1;

                var result = await _userService.DeleteAsync(id, userId);

                if (!result)
                {
                    return NotFound(                        $"User with Id {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while deleting user {UserId}.",
                    id);

                return StatusCode(
                    500,
                    "An error occurred while deleting the user.");
            }
        }
    }
}
