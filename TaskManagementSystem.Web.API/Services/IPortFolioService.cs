using TaskManagementSystem.Web.API.Models.DTOs;

namespace TaskManagementSystem.Web.API.Services
{
    public interface IPortFolioService
    {
        Task<IEnumerable<PortfolioResponseDto>> GetAllPortFolioAsync();
        Task<PortfolioResponseDto?> GetByIdPortFolioAsync(long portfolioId);
        Task<PortfolioResponseDto> CreatePortfolioDtoAsync(CreatePortfolioDto portfolioDto);
        Task<bool> UpdatePortfolioDtoAsync(long portfolioId, bool isActive, UpdatePortfolioDto updatePortfolioDto);
        Task<bool> DeleteAsync(long portfolioId, long ownerId);
    }
}