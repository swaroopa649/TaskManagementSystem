using TaskManagementSystem.Web.API.Models.Entities;

namespace TaskManagementSystem.Web.API.Respository
{
    public interface IPortFolioRepository
    {
        Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync();
        Task<Portfolio?> GetByIdPortfolioAsync(long portfolioId);
        Task<Portfolio> AddPortfolioAsync(Portfolio portfolio);
        Task<Portfolio?> UpdatePortfolioAsync(long portfolioId, Portfolio portfolio);
        Task<bool> DeletePortfolioAsync(long portfolioId);
    }
}

