using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Services;
using TaskManagementSystem.Web.API.Utility;

namespace TaskManagementSystem.Web.API.Controllers
{
    [ApiController]
    [TMSAuthorizeAttribute]
    [Route("api/[controller]")]
    public class PortFoliosController : ControllerBase
    {
        private readonly IPortFolioService _portfolioService;
        private readonly ILogger<PortFoliosController> _logger;

        public PortFoliosController(IPortFolioService portfolioService, ILogger<PortFoliosController> logger)
        {
            _portfolioService = portfolioService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortfolioResponseDto>>> GetAllPortfolios()
        {
            try
            {
                _logger.LogInformation("Getting all portfolios.");

                var portfolios = await _portfolioService.GetAllPortFolioAsync();

                return Ok(portfolios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all portfolios.");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving portfolios.");
            }
        }

        [HttpGet("{portfolioId:long}")]
        public async Task<ActionResult<PortfolioResponseDto>> GetPortfolioById(long portfolioId)
        {
            try
            {
                _logger.LogInformation("Getting portfolio with Id: {PortfolioId}", portfolioId);

                var portfolio = await _portfolioService.GetByIdPortFolioAsync(portfolioId);

                if (portfolio == null)
                {
                    _logger.LogWarning("Portfolio with Id {PortfolioId} was not found.", portfolioId);

                    return NotFound($"Portfolio with Id {portfolioId} not found.");
                }

                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting portfolio with Id: {PortfolioId}", portfolioId);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving the portfolio.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PortfolioResponseDto>> CreatePortfolio([FromBody] CreatePortfolioDto createPortfolioDto)
        {
            try
            {
                _logger.LogInformation("Creating portfolio with Code: {PortfolioCode}", createPortfolioDto.Code);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdPortfolio = await _portfolioService.CreatePortfolioDtoAsync(createPortfolioDto);

                _logger.LogInformation("Portfolio created successfully with Id: {PortfolioId}", createdPortfolio.PortfolioId);

                return CreatedAtAction(
                    nameof(GetPortfolioById),
                    new { portfolioId = createdPortfolio.PortfolioId },
                    createdPortfolio);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Unable to create portfolio with Code: {PortfolioCode}", createPortfolioDto.Code);

                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating portfolio.");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while creating the portfolio.");
            }
        }

        [HttpPut("{portfolioId:long}")]
        public async Task<IActionResult> UpdatePortfolio(long portfolioId, [FromQuery] bool isActive, [FromBody] UpdatePortfolioDto updatePortfolioDto)
        {
            try
            {
                _logger.LogInformation("Updating portfolio with Id: {PortfolioId}", portfolioId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var success = await _portfolioService.UpdatePortfolioDtoAsync(portfolioId, isActive, updatePortfolioDto);

                if (!success)
                {
                    _logger.LogWarning("Portfolio with Id {PortfolioId} was not found for update.", portfolioId);

                    return NotFound($"Portfolio with Id {portfolioId} not found.");
                }

                _logger.LogInformation("Portfolio with Id {PortfolioId} updated successfully.", portfolioId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating portfolio with Id: {PortfolioId}", portfolioId);

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while updating the portfolio.");
            }
        }

        [HttpDelete("{portfolioId:long}")]
        public async Task<IActionResult> DeletePortfolio(long portfolioId, [FromQuery] long ownerId)
        {
            try
            {
                _logger.LogInformation("Deleting portfolio with Id: {PortfolioId} for Owner: {OwnerId}", portfolioId, ownerId);

                var success = await _portfolioService.DeleteAsync(portfolioId, ownerId);

                if (!success)
                {
                    _logger.LogWarning("Portfolio with Id {PortfolioId} was not found or ownership mismatch.", portfolioId);

                    return NotFound($"Portfolio with Id {portfolioId} not found or unauthorized.");
                }

                _logger.LogInformation("Portfolio with Id {PortfolioId} deleted successfully.", portfolioId);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting portfolio with Id: {PortfolioId}", portfolioId);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while deleting the portfolio.");
            }
        }
    }
}