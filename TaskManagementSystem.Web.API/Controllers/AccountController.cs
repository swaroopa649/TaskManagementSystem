using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Services;

namespace TaskManagementSystem.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserLoginDto userLoginDto)
        {
            var response = await _accountService.AuthenticateAsync(userLoginDto);

            if (!response.IsValidUser || !response.IsValidPassword)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
    }
}
