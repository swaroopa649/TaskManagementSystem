using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.API.Models.Entities
{
    [Table("TeamMember")]
    public class TeamMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long TeamId { get; set; }

        public long UserId { get; set; }

        public long AssignedBy { get; set; }

        public DateTimeOffset AssignedOn { get; set; } = DateTimeOffset.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        [ForeignKey(nameof(TeamId))]
        public virtual Team? Team { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        [ForeignKey(nameof(AssignedBy))]
        public virtual User? AssignedByUser { get; set; }
    }
}