using Microsoft.EntityFrameworkCore.Metadata;

namespace TaskManagementSystem.Web.API.Models.DTOs
{
    public class UserResponseDto
    {
        public long Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public long? RoleId { get; set; }

        public string? RoleName { get; set; }

        public string? RoleCode { get; set; }

        public DateTimeOffset? LastLogin { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }
    }

    public class CreateUserDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public long? RoleId { get; set; }

        public string Password { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    public class UpdateUserDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public long? RoleId { get; set; }

        public bool IsActive { get; set; }
    }

    public class UserAuthenticationDTO : UserResponseDto
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
