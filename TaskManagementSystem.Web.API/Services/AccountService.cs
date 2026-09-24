using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskManagementSystem.Web.API.Models.DTOs;

namespace TaskManagementSystem.Web.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;
        private readonly PasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public AccountService(IUserService userService, PasswordService passwordService, IConfiguration configuration)
        {
            _userService = userService;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO> AuthenticateAsync(UserLoginDto userLoginDto)
        {
            var authResponse = new AuthResponseDTO();

            var user = await _userService.GetByUserNameAsync(userLoginDto.UserName);

            if (user == null)
            {
                authResponse.IsValidUser = false;
                authResponse.IsValidPassword = false;
                authResponse.Message = "Invalid email or password.";

                return authResponse;
            }

            var isPasswordValid = _passwordService.VerifyPassword(
                userLoginDto.Password,
                user.PasswordHash,
                user.PasswordSalt);

            if (!isPasswordValid)
            {
                authResponse.IsValidUser = true;
                authResponse.IsValidPassword = false;
                authResponse.Message = "Invalid email or password.";

                return authResponse;
            }

            if (!user.IsActive)
            {
                authResponse.IsValidUser = true;
                authResponse.IsValidPassword = true;
                authResponse.Message = "Login unsuccessful because the account is inactive.";

                return authResponse;
            }

            authResponse.IsValidUser = true;
            authResponse.IsValidPassword = true;
            authResponse.Message = "Login successful";

            authResponse.ApplicationUser = new ApplicationUser
            {
                Email = user.Email,
                FirstName = user.FirstName,
                IsActive = user.IsActive,
                LastName = user.LastName,
                Phone = user.Phone,
                RoleId = user.RoleId ?? 0,
                UserId = user.Id
            };

            // Generate JWT
            authResponse.JwtToken = GenerateJwtToken(user);

            return authResponse;
        }

        private string GenerateJwtToken(UserAuthenticationDTO user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");

            var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");

            var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");

            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(                    ClaimTypes.NameIdentifier,                    user.Id.ToString()),

                new Claim(                    ClaimTypes.Email,                    user.Email),

                new Claim(                    ClaimTypes.Name,                    $"{user.FirstName} {user.LastName}"),

                new Claim(                    ClaimTypes.Role,                    user.RoleCode ?? string.Empty),

                new Claim(                    "RoleId",                    (user.RoleId ?? 0).ToString()),

                new Claim(                    "FirstName",                    user.FirstName),

                new Claim(                    "LastName",                    user.LastName)
            };

            var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(expiryMinutes), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}