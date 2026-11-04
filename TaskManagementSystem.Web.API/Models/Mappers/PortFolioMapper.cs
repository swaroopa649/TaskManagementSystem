using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Models.Mappers
{
    public static class PortfolioMapper
    {
        public static PortfolioResponseDto ToDto(this Portfolio portfolio)
        {
            return new PortfolioResponseDto
            {
                PortfolioId = portfolio.PortfolioId,
                Name = portfolio.Name,
                Code = portfolio.Code,
                Description = portfolio.Description,
                OwnerId = portfolio.OwnerId,
                CreatedBy = portfolio.CreatedBy,
                CreatedOn = portfolio.CreatedOn,
                ModifiedBy = portfolio.ModifiedBy,
                ModifiedOn = portfolio.ModifiedOn,
                IsActive = portfolio.IsActive,
                CreatedDate = portfolio.CreatedDate
            };
        }

        public static Portfolio ToEntity(this CreatePortfolioDto dto)
        {
            return new Portfolio
            {
                Name = dto.Name,
                Code = dto.Code,
                Description = dto.Description,
                OwnerId = dto.OwnerId,
                IsActive = dto.IsActive
            };
        }

        public static void UpdateEntity(this UpdatePortfolioDto dto, Portfolio portfolio)
        {
            portfolio.Name = dto.Name;
            portfolio.Code = dto.Code;
            portfolio.Description = dto.Description;
            portfolio.OwnerId = dto.OwnerId;
            portfolio.IsActive = dto.IsActive;
        }
    }
}