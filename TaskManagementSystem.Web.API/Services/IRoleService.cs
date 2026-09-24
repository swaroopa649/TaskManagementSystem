using TaskManagementSystem.Web.API.Models.DTOs;

namespace TaskManagementSystem.Web.API.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponseDto>> GetAllAsync();
        Task<RoleResponseDto?> GetByIdAsync(long id);
        Task<RoleResponseDto> CreateAsync(CreateRoleDto dto, long userId);
        Task<RoleResponseDto?> UpdateAsync(
            long id,
            UpdateRoleDto dto,
            long userId);
        Task<bool> UpdateStatusAsync(
            long id,
            bool isActive,
            long userId);
        Task<bool> DeleteAsync(long id, long userId);
    }
}
