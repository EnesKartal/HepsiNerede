using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Campaign.CreateCampaign;
using HepsiNerede.Services;
using Moq;

namespace HepsiNerede.Tests
{
    public class CampaignServiceTests
    {
        [Fact]
        public void GetCampaignByName_ShouldReturnCampaign_WhenCampaignExists()
        {
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var campaignService = new CampaignService(campaignRepositoryMock.Object);

            var expectedCampaign = new Campaign
            {
                Name = "SummerSale",
                ProductCode = "P001",
                Duration = 24,
                PriceManipulationLimit = 10,
                TargetSalesCount = 50,
                Id = 1
            };

            campaignRepositoryMock.Setup(repo => repo.GetCampaignByName("SummerSale")).Returns(expectedCampaign);

            var actualCampaign = campaignService.GetCampaignByName("SummerSale");

            Assert.NotNull(actualCampaign);
            Assert.Equal(expectedCampaign.Name, actualCampaign.Name);
            Assert.Equal(expectedCampaign.ProductCode, actualCampaign.ProductCode);
            Assert.Equal(expectedCampaign.Duration, actualCampaign.Duration);
            Assert.Equal(expectedCampaign.PriceManipulationLimit, actualCampaign.PriceManipulationLimit);
            Assert.Equal(expectedCampaign.TargetSalesCount, actualCampaign.TargetSalesCount);

            campaignRepositoryMock.Verify(repo => repo.GetCampaignByName("SummerSale"), Times.Once);
        }

        [Fact]
        public void GetCampaignByName_ShouldReturnNull_WhenCampaignDoesNotExist()
        {
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var campaignService = new CampaignService(campaignRepositoryMock.Object);

            var actualCampaign = campaignService.GetCampaignByName("NonExistingCampaign");

            Assert.Null(actualCampaign);

            campaignRepositoryMock.Verify(repo => repo.GetCampaignByName("NonExistingCampaign"), Times.Once);
        }

        [Fact]
        public void CreateCampaign_ShouldCreateCampaignAndReturnIt()
        {
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            var campaignService = new CampaignService(campaignRepositoryMock.Object);

            var createCampaignRequest = new CreateCampaignRequestDTO
            {
                Name = "WinterSale",
                ProductCode = "P002",
                Duration = 48,
                PMLimit = 15,
                TSCount = 75
            };

            campaignRepositoryMock.Setup(repo => repo.CreateCampaign(It.IsAny<Campaign>())).Returns<Campaign>(c => c);

            var createdCampaign = campaignService.CreateCampaign(createCampaignRequest);

            Assert.NotNull(createdCampaign);
            Assert.Equal(createCampaignRequest.Name, createdCampaign.Name);
            Assert.Equal(createCampaignRequest.ProductCode, createdCampaign.ProductCode);
            Assert.Equal(createCampaignRequest.Duration, createdCampaign.Duration);
            Assert.Equal(createCampaignRequest.PMLimit, createdCampaign.PriceManipulationLimit);
            Assert.Equal(createCampaignRequest.TSCount, createdCampaign.TargetSalesCount);

            campaignRepositoryMock.Verify(repo => repo.CreateCampaign(It.IsAny<Campaign>()), Times.Once);
        }
    }
}
