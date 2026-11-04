using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Web.API.Models.Entities
{
    [Table("Portfolio")]
    public class Portfolio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Crucial for IDENTITY(1,1) tables
        public long PortfolioId { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? Name { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? Code { get; set; }

        [Column(TypeName = "text")]
        public string? Description { get; set; }

        public long OwnerId { get; set; }

        public long? CreatedBy { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}