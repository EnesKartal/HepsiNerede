using HepsiNerede.Application.DTOs.Campaign.CreateCampaign;
using HepsiNerede.Application.Services.Campaign;
using HepsiNerede.Application.Services.Order;
using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.Domain.Aggregates.CampaignAggregate;
using HepsiNerede.Domain.Aggregates.OrderAggregate;
using HepsiNerede.Domain.Entities;
using HepsiNerede.Infrastructure.Repositories;
using HepsiNerede.Tests.Helpers;
using Moq;
using System.Diagnostics;

namespace HepsiNerede.Tests
{
    public class CampaignServiceTests
    {
        [Fact]
        public async void CreateCampaign_ShouldReturnCampaign()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var campaignRepository = new CampaignRepository(dbContextMock);

            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var campaignRepositoryMock = new Mock<ICampaignRepository>();
            campaignRepositoryMock.Setup(x => x.CreateCampaignAsync(It.IsAny<Campaign>()))
                                 .Returns<Campaign>(campaign => campaignRepository.CreateCampaignAsync(campaign));

            var createCampaignRequestDTO = new CreateCampaignRequestDTO
            {
                Name = "TestCampaign",
                ProductCode = "TestProductCode",
                Duration = 5,
                PMLimit = 20,
                TSCount = 100
            };

            var currentTime = timeSimulationServiceMock.Object.GetCurrentTime();

            var createdCampaign = await campaignRepositoryMock.Object.CreateCampaignAsync(new Campaign
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

            campaignRepositoryMock.Verify(repo => repo.CreateCampaignAsync(It.Is<Campaign>(c => c == createdCampaign)), Times.Once);
        }


        [Fact]
        public async void GetCampaign_ShouldReturnCampaignResponseDTO()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var campaignRepository = new CampaignRepository(dbContextMock);

            var timeSimulationService = new TimeSimulationService();
            var orderService = new OrderService(new OrderRepository(dbContextMock), timeSimulationService);

            var campaignService = new CampaignService(campaignRepository, timeSimulationService);
            var campaignOrderService = new CampaignOrderService(campaignService, orderService);

            var createCampaignRequestDTO = new CreateCampaignRequestDTO
            {
                Name = "TestCampaign",
                ProductCode = "TestProductCode",
                Duration = 5,
                PMLimit = 20,
                TSCount = 100
            };

            var createdCampaign = await campaignService.CreateCampaignAsync(new CreateCampaignRequestDTO
            {
                Duration = createCampaignRequestDTO.Duration,
                Name = createCampaignRequestDTO.Name,
                PMLimit = createCampaignRequestDTO.PMLimit,
                ProductCode = createCampaignRequestDTO.ProductCode,
                TSCount = createCampaignRequestDTO.TSCount
            });

            Debug.WriteLine(createdCampaign.CreatedAt);
            var campaignResult = campaignOrderService.GetCampaignByNameAsync(createCampaignRequestDTO.Name);

            Assert.NotNull(createdCampaign);
            Assert.NotNull(campaignResult);

            var createOrderRequestDTO = new CreateOrderDTO
            {
                ProductCode = createCampaignRequestDTO.ProductCode,
                Quantity = 10,
                TotalPrice = 100
            };

            var createdOrder = await orderService.CreateOrderAsync(createOrderRequestDTO);

            var campaignResultWithOrders = await campaignOrderService.GetCampaignByNameAsync(createCampaignRequestDTO.Name);
            Assert.NotNull(campaignResultWithOrders);
            Assert.NotNull(createdOrder);

            Assert.Equal(createdOrder.Quantity, campaignResultWithOrders.TotalSales);
        }
    }
}
