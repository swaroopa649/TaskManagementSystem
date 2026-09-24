namespace TaskManagementSystem.Web.API.Models.DTOs
{
    public class TaskResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public long? TeamId { get; set; }
        public string? TeamName { get; set; }
        public long? AssigneeId { get; set; }
        public string? AssigneeName { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public bool IsActive { get; set; }
        public long CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public List<TaskCommentResponseDto> Comments { get; set; } = new();
    }

    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long StatusId { get; set; } = 1; // Default to 'To Do'
        public string Priority { get; set; } = "Medium";
        public long? TeamId { get; set; }
        public long? AssigneeId { get; set; }
        public DateTimeOffset? DueDate { get; set; }
    }

    public class UpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long StatusId { get; set; }
        public string Priority { get; set; } = "Medium";
        public long? TeamId { get; set; }
        public long? AssigneeId { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateTaskStatusDto
    {
        public long StatusId { get; set; }
    }
}