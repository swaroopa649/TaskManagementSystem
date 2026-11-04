using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Web.API.Controllers;
using TaskManagementSystem.Web.API.Models.DTOs;
using TaskManagementSystem.Web.API.Services;

namespace TaskManagementSystem.Web.Tests.Controllers
{
    public class PortFoliosControllerTests
    {
        private readonly Mock<IPortFolioService> _mockPortfolioService;
        private readonly PortFoliosController _controller;

        public PortFoliosControllerTests()
        {
            _mockPortfolioService = new Mock<IPortFolioService>();
            _controller = new PortFoliosController(_mockPortfolioService.Object);
        }

        [Fact]
        public async Task GetAllPortfoliosAsync_RetrinWithPortfolios_WithListOfPortfolios()
        {
            // Arrange
            var mockPortfolios = new List<PortfolioResponseDto>
            {
                new PortfolioResponseDto { PortfolioId = 1, Name = "Portfolio 1" },
                new PortfolioResponseDto { PortfolioId = 2, Name = "Portfolio 2" }
            };
            _mockPortfolioService.Setup(service => service.GetAllPortFolioAsync()).ReturnsAsync(mockPortfolios);
            // Act
            var response = await _controller.GetAllPortfoliosAsync();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            var returnValue = Assert.IsType<List<PortfolioResponseDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllPortfoliosAsync_ReturnsOkWithEmptyList()
        {
            // Arrange
            var portfolios = new List<PortfolioResponseDto>();

            _mockPortfolioService.Setup(x => x.GetAllPortFolioAsync()).ReturnsAsync(portfolios);

            // Act
            var result = await _controller.GetAllPortfoliosAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsAssignableFrom<IEnumerable<PortfolioResponseDto>>(okResult.Value);

            Assert.Empty(response);

            Assert.Equal(0, response.Count());
        }


        [Fact]
        public async Task GetPortfolioById_ReturnsOk_WhenPortfolioExists()
        {
            // Arrange
            long portfolioId = 1;

            var portfolio = new PortfolioResponseDto
            {
                PortfolioId = portfolioId,
                Name = "Portfolio One",
                Code = "PF001",
                OwnerId = 100,
                IsActive = true
            };

            _mockPortfolioService
                .Setup(x => x.GetByIdPortFolioAsync(portfolioId))
                .ReturnsAsync(portfolio);

            // Act
            var result = await _controller.GetPortfolioById(
                portfolioId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(
                result.Result);

            var response = Assert.IsType<PortfolioResponseDto>(
                okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(portfolioId, response.PortfolioId);
            Assert.Equal("Portfolio One", response.Name);

            _mockPortfolioService.Verify(x => x.GetByIdPortFolioAsync(portfolioId), Times.Once);
        }

        [Fact]
        public async Task GetPortfolioById_ReturnsNotFound_WhenPortfolioDoesNotExist()
        {
            // Arrange
            long portfolioId = 999;

            _mockPortfolioService
                .Setup(x => x.GetByIdPortFolioAsync(portfolioId))
                .ReturnsAsync(
                    (PortfolioResponseDto?)null);

            // Act
            var result = await _controller.GetPortfolioById(
                portfolioId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

            _mockPortfolioService.Verify(
                x => x.GetByIdPortFolioAsync(portfolioId),
                Times.Once);
        }

        [Fact]
        public async Task GetPortfolioById_ReturnsBadRequest_WhenPortfolioIdIsLessThanOrEqualZero()
        {
            // Arrange
            long portfolioId = 0;

            // Act
            var result = await _controller.GetPortfolioById(portfolioId);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid portfolio ID.", badRequest.Value);

            // Service should NOT be called — the guard clause short-circuits.
            _mockPortfolioService.Verify(
                x => x.GetByIdPortFolioAsync(It.IsAny<long>()),
                Times.Never);
        }
    }
}
