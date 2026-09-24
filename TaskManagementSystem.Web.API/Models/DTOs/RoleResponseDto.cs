namespace TaskManagementSystem.Web.API.Models.DTOs
{
    public class RoleResponseDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public bool? IsActive { get; set; }
    }

    public class CreateRoleDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateRoleDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
