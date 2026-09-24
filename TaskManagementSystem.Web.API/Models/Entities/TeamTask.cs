using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.API.Models.Entities
{
    [Table("Task")]
    public class TeamTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "varchar(max)")]
        public string? Description { get; set; }

        public long StatusId { get; set; } = 1; // Default to 1 ('To Do')

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Priority { get; set; } = "Medium";

        public long? TeamId { get; set; }

        public long? AssigneeId { get; set; }

        public DateTimeOffset? DueDate { get; set; }

        public bool IsActive { get; set; } = true;

        public long CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public long? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(StatusId))]
        public virtual TaskStatus? Status { get; set; }

        [ForeignKey(nameof(TeamId))]
        public virtual Team? Team { get; set; }

        [ForeignKey(nameof(AssigneeId))]
        public virtual User? Assignee { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual User? Creator { get; set; }

        public virtual ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    }
}