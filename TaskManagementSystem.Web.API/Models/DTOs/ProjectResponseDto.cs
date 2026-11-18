namespace TaskManagementSystem.Web.API.Models.DTOs
{
    public class ProjectResponseDto
    {
        public long ProjectId { get; set; }
        public string? ProjectKey { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long LeadUserId { get; set; }
        public long? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }

    public class CreateProjectDto
    {
        public string ProjectKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long LeadUserId { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class UpdateProjectDto
    {
        public string ProjectKey { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long LeadUserId { get; set; }
        public bool IsActive { get; set; }
    }
}