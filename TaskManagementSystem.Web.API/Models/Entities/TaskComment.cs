using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.API.Models.Entities
{
    [Table("TaskComment")]
    public class TaskComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long TaskId { get; set; }

        public long UserId { get; set; }

        [Required]
        [Column(TypeName = "varchar(max)")]
        public string Comment { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public long CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public long? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(TaskId))]
        public virtual TeamTask? Task { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}