using HepsiNerede.Application.DTOs.Campaign.CreateCampaign;
using HepsiNerede.Application.DTOs.Campaign.GetCampaign;
using HepsiNerede.Application.Services.Campaign;
using HepsiNerede.WebApp.Common;
using HepsiNerede.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HepsiNerede.Tests.Controllers
{
    public class CampaignControllerTests
    {
        [Fact]
        public async Task GetCampaignByNameAsync_WithValidName_ShouldReturnOkResult()
        {
            var campaignOrderServiceMock = new Mock<ICampaignOrderService>();
            var campaignController = new CampaignController(null, campaignOrderServiceMock.Object);

            var expectedCampaign = new GetCampaignResponseDTO
            {
                AverageItemPrice = 10,
                Status = "Active",
                Turnover = 100,
                TotalSales = 10 
            };

            campaignOrderServiceMock.Setup(x => x.GetCampaignByNameAsync(It.IsAny<string>())).ReturnsAsync(expectedCampaign);

            var result = await campaignController.GetCampaignByNameAsync("validCampaignName");

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<GetCampaignResponseDTO>>(okResult.Value);
            Assert.Equal(expectedCampaign, apiResponse.Data);
        }

        [Fact]
        public async Task GetCampaignByNameAsync_WithInvalidName_ShouldReturnNotFoundResult()
        {
            var campaignOrderServiceMock = new Mock<ICampaignOrderService>();
            var campaignController = new CampaignController(null, campaignOrderServiceMock.Object);

            campaignOrderServiceMock.Setup(x => x.GetCampaignByNameAsync(It.IsAny<string>())).ReturnsAsync((GetCampaignResponseDTO)null);

            var result = await campaignController.GetCampaignByNameAsync("nonExistingCampaignName");

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Campaign with name 'nonExistingCampaignName' not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateProductAsync_WithValidCampaign_ShouldReturnOkResult()
        {
            var campaignServiceMock = new Mock<ICampaignService>();
            var campaignController = new CampaignController(campaignServiceMock.Object, null);

            var createCampaignRequest = new CreateCampaignRequestDTO
            {
                Duration = 5,
                Name = "C1",
                PMLimit = 20,
                ProductCode = "P1",
                TSCount = 100
            };

            var createdCampaign = new Domain.Entities.Campaign
            {
                Duration = 5,
                Name = "C1",
                PriceManipulationLimit = 20,
                ProductCode = "P1",
                TargetSalesCount = 100
            };

            campaignServiceMock.Setup(x => x.CreateCampaignAsync(It.IsAny<CreateCampaignRequestDTO>())).ReturnsAsync(createdCampaign);

            var result = await campaignController.CreateProductAsync(createCampaignRequest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<CreateCampaignResponseDTO>>(okResult.Value);
            Assert.Equal(createdCampaign.Duration, apiResponse.Data.Duration);
            Assert.Equal(createdCampaign.Name, apiResponse.Data.Name);
            Assert.Equal(createdCampaign.PriceManipulationLimit, apiResponse.Data.Limit);
            Assert.Equal(createdCampaign.ProductCode, apiResponse.Data.Product);
            Assert.Equal(createdCampaign.TargetSalesCount, apiResponse.Data.TargetSalesCount);

        }

        [Fact]
        public async Task CreateProductAsync_WithInvalidCampaign_ShouldReturnBadRequestResult()
        {
            var campaignServiceMock = new Mock<ICampaignService>();
            var campaignController = new CampaignController(campaignServiceMock.Object, null);

            var result = await campaignController.CreateProductAsync(null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Campaign is required.", badRequestResult.Value);
        }
    }
}