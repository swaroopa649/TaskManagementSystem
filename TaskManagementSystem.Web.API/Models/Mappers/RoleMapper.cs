using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Models.Mappers
{
    public static class RoleMapper
    {
        public static RoleResponseDto ToDto(this Role role)
        {
            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                Code = role.Code,
                IsActive = role.IsActive
            };
        }

        public static Role ToEntity(this CreateRoleDto dto)
        {
            return new Role
            {
                Name = dto.Name,
                Code = dto.Code,
                IsActive = dto.IsActive
            };
        }

        public static void UpdateEntity(this UpdateRoleDto dto, Role role)
        {
            role.Name = dto.Name;
            role.Code = dto.Code;
            role.IsActive = dto.IsActive;
        }
    }
}
}
