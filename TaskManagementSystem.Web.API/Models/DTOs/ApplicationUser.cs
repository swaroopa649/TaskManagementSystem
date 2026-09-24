namespace TaskManagementSystem.Web.API.Models.DTOs
{
    public class ApplicationUser
    {
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public long RoleId { get; set; }
        public bool IsActive { get; set; }
        public RoleResponseDto? RoleInfo { get; set; }
    }
}
