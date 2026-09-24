using TaskManagementSystem.Web.API.Models.DTOs;

namespace TaskManagementSystem.Web.API.Services
{
    public interface IAccountService
    {
        Task<AuthResponseDTO> AuthenticateAsync(UserLoginDto userLoginDto);
    }
}
