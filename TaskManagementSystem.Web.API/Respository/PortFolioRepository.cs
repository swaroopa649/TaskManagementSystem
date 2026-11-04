using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Web.API.Data;
using TaskManagementSystem.Web.API.Models.Entities;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Repository
{
    public class PortfolioRepository : IPortFolioRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<PortfolioRepository> _logger;

        public PortfolioRepository(ApplicationDBContext context, ILogger<PortfolioRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Portfolio>> GetAllPortfoliosAsync()
        {
            try
            {
                return await _context.Set<Portfolio>().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching all portfolios.");
                throw;
            }
        }

        public async Task<Portfolio?> GetByIdPortfolioAsync(long portfolioId)
        {
            try
            {
                return await _context.Set<Portfolio>().FindAsync(portfolioId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching portfolio with ID {PortfolioId}.", portfolioId);
                throw;
            }
        }

        public async Task<Portfolio> AddPortfolioAsync(Portfolio portfolio)
        {
            try
            {
                portfolio.CreatedDate = DateTime.UtcNow;
                _context.Set<Portfolio>().Add(portfolio);
                await _context.SaveChangesAsync();
                return portfolio;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error occurred while adding a new portfolio.");
                throw;
            }
        }

        public async Task<Portfolio?> UpdatePortfolioAsync(long portfolioId, Portfolio portfolio)
        {
            try
            {
                var existingPortfolio = await _context.Set<Portfolio>().FindAsync(portfolioId);
                if (existingPortfolio == null)
                {
                    return null;
                }

                existingPortfolio.Name = portfolio.Name;
                existingPortfolio.Code = portfolio.Code;
                existingPortfolio.Description = portfolio.Description;
                existingPortfolio.OwnerId = portfolio.OwnerId;
                existingPortfolio.ModifiedBy = portfolio.ModifiedBy;
                existingPortfolio.ModifiedOn = portfolio.ModifiedOn;
                existingPortfolio.IsActive = portfolio.IsActive;

                await _context.SaveChangesAsync();
                return existingPortfolio;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error occurred while updating portfolio with ID {PortfolioId}.", portfolioId);
                throw;
            }
        }

        public async Task<bool> DeletePortfolioAsync(long portfolioId)
        {
            try
            {
                var portfolio = await _context.Set<Portfolio>().FindAsync(portfolioId);
                if (portfolio == null)
                {
                    return false;
                }

                _context.Set<Portfolio>().Remove(portfolio);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database error occurred while deleting portfolio with ID {PortfolioId}.", portfolioId);
                throw;
            }
        }
    }
}