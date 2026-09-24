namespace TaskManagementSystem.Web.API.Models.DTOs
{
    public class TeamResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public List<TeamMemberResponseDto> Members { get; set; } = new();
    }

    public class CreateTeamDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long ManagerId { get; set; }
    }

    public class UpdateTeamDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long ManagerId { get; set; }
        public bool IsActive { get; set; }
    }

    public class AssignTeamMemberDto
    {
        public long TeamId { get; set; }
        public long UserId { get; set; }
    }

    public class TeamMemberResponseDto
    {
        public long Id { get; set; }
        public long TeamId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTimeOffset AssignedOn { get; set; }
        public bool IsActive { get; set; }
    }
}