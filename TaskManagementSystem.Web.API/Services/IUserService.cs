using TaskManagementSystem.Web.API.Models.DTOs;

namespace TaskManagementSystem.Web.API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();

        Task<UserResponseDto?> GetByIdAsync(long id);

        Task<UserResponseDto> CreateAsync(CreateUserDto dto, long userId);

        Task<UserResponseDto?> UpdateAsync(long id, UpdateUserDto dto, long userId);

        Task<bool> UpdateStatusAsync(long id, bool isActive, long userId);

        Task<bool> DeleteAsync(long id, long userId);
    }
}
