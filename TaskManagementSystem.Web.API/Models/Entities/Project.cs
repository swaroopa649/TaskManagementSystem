using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.API.Models.Entities
{
    [Table("Project")]
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProjectId { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? ProjectKey { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? Name { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? Description { get; set; }

        public long LeadUserId { get; set; }

        public long? CreatedBy { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}