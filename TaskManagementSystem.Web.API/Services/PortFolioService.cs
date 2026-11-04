using Microsoft.Extensions.Logging;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Mappers;
using TaskManagementSystem.Web.API.Respository;

namespace TaskManagementSystem.Web.API.Services
{
    public class PortFolioService : IPortFolioService
    {
        private readonly IPortFolioRepository _portfolioRepository;
        private readonly ILogger<PortFolioService> _logger;

        public PortFolioService(IPortFolioRepository portfolioRepository, ILogger<PortFolioService> logger)
        {
            _portfolioRepository = portfolioRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<PortfolioResponseDto>> GetAllPortFolioAsync()
        {
            try
            {
                var portfolios = await _portfolioRepository.GetAllPortfoliosAsync();
                return portfolios.Select(p => p.ToDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error while fetching all portfolios.");
                throw;
            }
        }

        public async Task<PortfolioResponseDto?> GetByIdPortFolioAsync(long portfolioId)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetByIdPortfolioAsync(portfolioId);
                return portfolio?.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error while fetching portfolio with ID {PortfolioId}.", portfolioId);
                throw;
            }
        }

        public async Task<PortfolioResponseDto> CreatePortfolioDtoAsync(CreatePortfolioDto portfolioDto)
        {
            try
            {
                var portfolioEntity = portfolioDto.ToEntity();
                portfolioEntity.CreatedDate = DateTime.UtcNow;
                portfolioEntity.CreatedOn = DateTimeOffset.UtcNow;

                var createdPortfolio = await _portfolioRepository.AddPortfolioAsync(portfolioEntity);
                return createdPortfolio.ToDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error while creating a portfolio.");
                throw;
            }
        }

        public async Task<bool> UpdatePortfolioDtoAsync(long portfolioId, bool isActive, UpdatePortfolioDto updatePortfolioDto)
        {
            try
            {
                if (updatePortfolioDto == null)
                {
                    return false;
                }

                var existingPortfolio = await _portfolioRepository.GetByIdPortfolioAsync(portfolioId);
                if (existingPortfolio == null)
                {
                    return false;
                }

                updatePortfolioDto.UpdateEntity(existingPortfolio);
                existingPortfolio.IsActive = isActive;
                existingPortfolio.ModifiedOn = DateTimeOffset.UtcNow;

                var updatedPortfolio = await _portfolioRepository.UpdatePortfolioAsync(portfolioId, existingPortfolio);
                return updatedPortfolio != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error while updating portfolio with ID {PortfolioId}.", portfolioId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long portfolioId, long ownerId)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetByIdPortfolioAsync(portfolioId);
                if (portfolio == null || portfolio.OwnerId != ownerId)
                {
                    return false;
                }

                return await _portfolioRepository.DeletePortfolioAsync(portfolioId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Service error while deleting portfolio with ID {PortfolioId}.", portfolioId);
                throw;
            }
        }
    }
}