using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Models.Entities;
using TaskManagementSystem.Web.API.Services;

namespace TaskManagementSystem.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortFoliosController : ControllerBase
    {
        private readonly IPortFolioService _portfolioService;

        public PortFoliosController(IPortFolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PortfolioResponseDto>>> GetAllPortfoliosAsync()
        {
            var portfolios = await _portfolioService.GetAllPortFolioAsync();
            return Ok(portfolios);
        }

        [HttpGet("{portfolioId:long}")]
        public async Task<ActionResult<PortfolioResponseDto>> GetPortfolioById(long portfolioId)
        {
            var portfolio = await _portfolioService.GetByIdPortFolioAsync(portfolioId);
            if (portfolio == null)
            {
                return NotFound();
            }
            return Ok(portfolio);
        }

        [HttpPost]
        public async Task<ActionResult<PortfolioResponseDto>> CreatePortfolioAsync([FromBody] CreatePortfolioDto createPortfolioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPortfolio = await _portfolioService.CreatePortfolioDtoAsync(createPortfolioDto);

            // FIXED: Changed 'id' to 'portfolioId' to match the route definition
            return CreatedAtAction(nameof(GetPortfolioById), new { portfolioId = createdPortfolio.PortfolioId }, createdPortfolio);
        }

        [HttpPut("{portfolioId:long}")]
        public async Task<IActionResult> UpdatePortfolio(long portfolioId, [FromQuery] bool isActive, [FromBody] UpdatePortfolioDto updatePortfolioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // FIX: Changed 'id' to 'portfolioId' to match method argument and route template
            var success = await _portfolioService.UpdatePortfolioDtoAsync(portfolioId, isActive, updatePortfolioDto);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{portfolioId:long}")]
        public async Task<IActionResult> DeletePortfolio(long portfolioId, [FromQuery] long ownerId)
        {
            var success = await _portfolioService.DeleteAsync(portfolioId, ownerId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}