using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.API.Models.Entities
{
    [Table("TaskStatus")]
    public class TaskStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public long? CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public long? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }
    }
}