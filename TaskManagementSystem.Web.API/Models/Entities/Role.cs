using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.API.Models.Entities
{
    [Table("Role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? Name { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? Code { get; set; }

        public long? CreatedBy { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }
    }
}