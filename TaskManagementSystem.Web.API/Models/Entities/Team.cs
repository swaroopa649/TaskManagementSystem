using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.API.Models.Entities
{
    [Table("Team")]
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(150)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(500)")]
        public string? Description { get; set; }

        public long ManagerId { get; set; }

        public bool IsActive { get; set; } = true;

        public long? CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public long? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(ManagerId))]
        public virtual User? Manager { get; set; }

        public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}