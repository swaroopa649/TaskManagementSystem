namespace TaskManagementSystem.Web.API.Models.DTOs
{
    public class TaskCommentResponseDto
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; }
    }

    public class CreateTaskCommentDto
    {
        public long TaskId { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}