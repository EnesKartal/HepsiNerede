using HepsiNerede.Data;
using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Campaign.CreateCampaign;
using HepsiNerede.Services;
using HepsiNerede.Tests.Helpers;
using Moq;

namespace HepsiNerede.Tests
{
    public class CampaignServiceTests
    {
        [Fact]
        public void CreateCampaign_ShouldReturnCampaign()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var campaignRepository = new CampaignRepository(dbContextMock);

            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            campaignRepositoryMock.Setup(x => x.CreateCampaign(It.IsAny<Campaign>()))
                                 .Returns<Campaign>(campaign => campaignRepository.CreateCampaign(campaign));

            var createCampaignRequestDTO = new CreateCampaignRequestDTO
            {
                Name = "TestCampaign",
                ProductCode = "TestProductCode",
                Duration = 5,
                PMLimit = 20,
                TSCount = 100
            };

            var currentTime = timeSimulationServiceMock.Object.GetCurrentTime();

            var createdCampaign = campaignRepositoryMock.Object.CreateCampaign(new Campaign
            {
                Name = createCampaignRequestDTO.Name,
                ProductCode = createCampaignRequestDTO.ProductCode,
                Duration = createCampaignRequestDTO.Duration,
                PriceManipulationLimit = createCampaignRequestDTO.PMLimit,
                TargetSalesCount = createCampaignRequestDTO.TSCount,
                CreatedAt = currentTime
            });

            Assert.NotNull(createdCampaign);
            Assert.Equal(createCampaignRequestDTO.Name, createdCampaign.Name);
            Assert.Equal(createCampaignRequestDTO.ProductCode, createdCampaign.ProductCode);
            Assert.Equal(createCampaignRequestDTO.Duration, createdCampaign.Duration);
            Assert.Equal(createCampaignRequestDTO.PMLimit, createdCampaign.PriceManipulationLimit);
            Assert.Equal(createCampaignRequestDTO.TSCount, createdCampaign.TargetSalesCount);
            Assert.Equal(currentTime, createdCampaign.CreatedAt);

            campaignRepositoryMock.Verify(repo => repo.CreateCampaign(It.Is<Campaign>(c => c == createdCampaign)), Times.Once);
        }


        [Fact]
        public void GetCampaign_ShouldReturnCampaignResponseDTO()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var campaignRepository = new CampaignRepository(dbContextMock);

            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            campaignRepositoryMock.Setup(x => x.GetCampaignByName(It.IsAny<string>()))
                                 .Returns<string>(name => campaignRepository.GetCampaignByName(name));
            campaignRepositoryMock.Setup(x => x.CreateCampaign(It.IsAny<Campaign>()))
                                 .Returns<Campaign>(campaign => campaignRepository.CreateCampaign(campaign));

            var createCampaignRequestDTO = new CreateCampaignRequestDTO
            {
                Name = "TestCampaign",
                ProductCode = "TestProductCode",
                Duration = 5,
                PMLimit = 20,
                TSCount = 100
            };

            var currentTime = timeSimulationServiceMock.Object.GetCurrentTime();

            var createdCampaign = campaignRepositoryMock.Object.CreateCampaign(new Campaign
            {
                Name = createCampaignRequestDTO.Name,
                ProductCode = createCampaignRequestDTO.ProductCode,
                Duration = createCampaignRequestDTO.Duration,
                PriceManipulationLimit = createCampaignRequestDTO.PMLimit,
                TargetSalesCount = createCampaignRequestDTO.TSCount,
                CreatedAt = currentTime
            });

            var campaignResult = campaignRepositoryMock.Object.GetCampaignByName(createCampaignRequestDTO.Name);

            Assert.NotNull(createdCampaign);
            Assert.NotNull(campaignResult);
            Assert.Equal(createCampaignRequestDTO.Name, campaignResult.Name);
            Assert.Equal(createCampaignRequestDTO.ProductCode, campaignResult.ProductCode);
            Assert.Equal(createCampaignRequestDTO.Duration, campaignResult.Duration);
            Assert.Equal(createCampaignRequestDTO.PMLimit, campaignResult.PriceManipulationLimit);
            Assert.Equal(createCampaignRequestDTO.TSCount, campaignResult.TargetSalesCount);
            Assert.Equal(currentTime, campaignResult.CreatedAt);

            campaignRepositoryMock.Verify(repo => repo.GetCampaignByName(It.Is<string>(c => c == campaignResult.Name)), Times.Once);
        }
    }
}
