namespace TaskManagementSystem.Web.API.Models.DTOs
{
    public class PortfolioResponseDto
    {
        public long PortfolioId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public long OwnerId { get; set; } // Changed from int to long
        public long? CreatedBy { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreatePortfolioDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long OwnerId { get; set; } // Changed from int to long
        public bool IsActive { get; set; } = true;
    }

    public class UpdatePortfolioDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long OwnerId { get; set; } // Changed from int to long
        public bool IsActive { get; set; }
    }
}