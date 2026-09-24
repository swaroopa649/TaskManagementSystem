using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Models.Mappers
{
    public static class UserMapper
    {
        public static UserResponseDto ToDto(this User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name,
                RoleCode = user.Role?.Code,
                LastLogin = user.LastLogin,
                IsActive = user.IsActive,
                CreatedOn = user.CreatedOn,
                ModifiedOn = user.ModifiedOn
            };
        }
        public static UserAuthenticationDTO ToAuthDto(this User user)
        {
            return new UserAuthenticationDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name,
                RoleCode = user.Role?.Code,
                LastLogin = user.LastLogin,
                IsActive = user.IsActive,
                CreatedOn = user.CreatedOn,
                ModifiedOn = user.ModifiedOn
            };
        }
        public static User ToEntity(this CreateUserDto dto, string passwordHash, string passwordSalt)
        {
            return new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                RoleId = dto.RoleId,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = dto.IsActive
            };
        }

        public static void UpdateEntity(this UpdateUserDto dto, User user)
        {
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.RoleId = dto.RoleId;
            user.IsActive = dto.IsActive;
        }
    }
}
