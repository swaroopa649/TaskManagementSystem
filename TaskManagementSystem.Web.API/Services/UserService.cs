using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Mappers;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordService _passwordService;

        public UserService(IUserRepository userRepository, PasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(x => x.ToDto());
        }

        public async Task<UserResponseDto?> GetByIdAsync(long id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null;

            return user.ToDto();
        }

        public async Task<UserResponseDto> CreateAsync(
            CreateUserDto dto,
            long userId)
        {
            var emailExists =
                await _userRepository.ExistsByEmailAsync(dto.Email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    $"User with email '{dto.Email}' already exists.");
            }

            if (dto.RoleId.HasValue)
            {
                var roleExists =
                    await _userRepository.RoleExistsAsync(
                        dto.RoleId.Value);

                if (!roleExists)
                {
                    throw new InvalidOperationException(
                        $"Role with Id {dto.RoleId} does not exist.");
                }
            }

            var passwordResult =
                _passwordService.HashPassword(dto.Password);

            var user = dto.ToEntity(
                passwordResult.Hash,
                passwordResult.Salt);

            user.Id = await GetNextIdAsync();

            user.CreatedBy = userId;
            user.CreatedOn = DateTimeOffset.UtcNow;

            var createdUser =
                await _userRepository.AddAsync(user);

            // Reload to get Role navigation property
            createdUser =
                await _userRepository.GetByIdAsync(
                    createdUser.Id) ?? createdUser;

            return createdUser.ToDto();
        }

        public async Task<UserResponseDto?> UpdateAsync(long id, UpdateUserDto dto, long userId)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null;

            var emailExists = await _userRepository.ExistsByEmailAsync(dto.Email, id);

            if (emailExists)
            {
                throw new InvalidOperationException($"User with email '{dto.Email}' already exists.");
            }

            if (dto.RoleId.HasValue)
            {
                var roleExists = await _userRepository.RoleExistsAsync(dto.RoleId.Value);

                if (!roleExists)
                {
                    throw new InvalidOperationException($"Role with Id {dto.RoleId} does not exist.");
                }
            }

            dto.UpdateEntity(user);

            user.ModifiedBy = userId;
            user.ModifiedOn = DateTimeOffset.UtcNow;

            await _userRepository.UpdateAsync(user);

            user = await _userRepository.GetByIdAsync(id) ?? user;

            return user.ToDto();
        }

        public async Task<bool> UpdateStatusAsync(long id, bool isActive, long userId)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            user.IsActive = isActive;
            user.ModifiedBy = userId;
            user.ModifiedOn = DateTimeOffset.UtcNow;

            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> DeleteAsync(long id, long userId)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            // Soft delete
            user.IsActive = false;
            user.ModifiedBy = userId;
            user.ModifiedOn = DateTimeOffset.UtcNow;

            await _userRepository.UpdateAsync(user);

            return true;
        }

        private async Task<long> GetNextIdAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Any() ? users.Max(x => x.Id) + 1 : 1;
        }

        public async Task<UserAuthenticationDTO> GetByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUserName(userName);
            if (user != null)
                return user.ToAuthDto();
            return null;
        }
    }
}
