using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Mappers;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllAsync()
        {
            var roles = await _roleRepository.GetAllAsync();

            return roles.Select(x => x.ToDto());
        }

        public async Task<RoleResponseDto?> GetByIdAsync(long id)
        {
            var role = await _roleRepository.GetByIdAsync(id);

            if (role == null)
                return null;

            return role.ToDto();
        }

        public async Task<RoleResponseDto> CreateAsync(
            CreateRoleDto dto,
            long userId)
        {
            var codeExists =
                await _roleRepository.ExistsByCodeAsync(dto.Code);

            if (codeExists)
                throw new InvalidOperationException(
                    $"Role with Code '{dto.Code}' already exists.");

            var role = dto.ToEntity();

            role.Id = await GetNextIdAsync();
            role.CreatedBy = userId;
            role.CreatedOn = DateTimeOffset.UtcNow;
            role.IsActive = true;

            var createdRole =
                await _roleRepository.AddAsync(role);

            return createdRole.ToDto();
        }

        public async Task<RoleResponseDto?> UpdateAsync(
            long id,
            UpdateRoleDto dto,
            long userId)
        {
            var role = await _roleRepository.GetByIdAsync(id);

            if (role == null)
                return null;

            var codeExists =
                await _roleRepository.ExistsByCodeAsync(dto.Code, id);

            if (codeExists)
                throw new InvalidOperationException(
                    $"Role with Code '{dto.Code}' already exists.");

            dto.UpdateEntity(role);

            role.ModifiedBy = userId;
            role.ModifiedOn = DateTimeOffset.UtcNow;

            await _roleRepository.UpdateAsync(role);

            return role.ToDto();
        }

        public async Task<bool> UpdateStatusAsync(
            long id,
            bool isActive,
            long userId)
        {
            var role = await _roleRepository.GetByIdAsync(id);

            if (role == null)
                return false;

            role.IsActive = isActive;
            role.ModifiedBy = userId;
            role.ModifiedOn = DateTimeOffset.UtcNow;

            await _roleRepository.UpdateAsync(role);

            return true;
        }

        public async Task<bool> DeleteAsync(
            long id,
            long userId)
        {
            var role = await _roleRepository.GetByIdAsync(id);

            if (role == null)
                return false;

            // Soft delete
            role.IsActive = false;
            role.ModifiedBy = userId;
            role.ModifiedOn = DateTimeOffset.UtcNow;

            await _roleRepository.UpdateAsync(role);

            return true;
        }

        private async Task<long> GetNextIdAsync()
        {
            var roles = await _roleRepository.GetAllAsync();

            return roles.Any()
                ? roles.Max(x => x.Id) + 1
                : 1;
        }
    }
}
