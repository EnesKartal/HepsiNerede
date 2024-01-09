using HepsiNerede.Application.Services.Order;
using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.Domain.Aggregates.OrderAggregate;
using HepsiNerede.Domain.Entities;
using HepsiNerede.Infrastructure.Repositories;
using HepsiNerede.Tests.Helpers;
using Moq;

namespace HepsiNerede.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public async void CreateOrder_CreatesOrderCorrectly()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var orderRepository = new OrderRepository(dbContextMock);
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(x => x.CreateOrderAsync(It.IsAny<Order>())).Returns<Order>(orderRepository.CreateOrderAsync);

            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();

            var orderService = new OrderService(orderRepositoryMock.Object, timeSimulationServiceMock.Object);

            var createOrderRequestDTO = new CreateOrderDTO
            {
                ProductCode = "TestProductCode",
                Quantity = 5,
                TotalPrice = 100
            };

            var currentTime = DateTime.Now;

            timeSimulationServiceMock.Setup(t => t.GetCurrentTime()).Returns(currentTime);

            var createdOrder = await orderService.CreateOrderAsync(createOrderRequestDTO);

            Assert.NotNull(createdOrder);
            Assert.Equal(createOrderRequestDTO.ProductCode, createdOrder.ProductCode);
            Assert.Equal(createOrderRequestDTO.Quantity, createdOrder.Quantity);
            Assert.Equal(currentTime, createdOrder.CreatedAt);
            Assert.Equal(createOrderRequestDTO.TotalPrice, createdOrder.TotalPrice);

            orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.Is<Order>(o => o == createdOrder)), Times.Once);
        }

        [Fact]
        public async void GetOrdersForCampaignProduct_ShouldReturnOrders()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();

            var productCode = "P001";
            var currentDate = timeSimulationServiceMock.Object.GetCurrentTime();
            var campaignEndTime = currentDate.AddHours(12);
            var orders = new Order[]
            {
            new Order { ProductCode = productCode, TotalPrice = 50.0m, Quantity = 2, CreatedAt = campaignEndTime.AddHours(-1) },
            new Order { ProductCode = productCode, TotalPrice = 30.0m, Quantity = 1, CreatedAt = campaignEndTime.AddMinutes(-30) }
            };

            orderRepositoryMock.Setup(x => x.GetOrdersForCampaignProductAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(orders);

            var orderService = new OrderService(orderRepositoryMock.Object, timeSimulationServiceMock.Object);

            var result = await orderService.GetOrdersForCampaignProductAsync(productCode, currentDate, campaignEndTime);

            Assert.NotNull(result);
            Assert.Equal(orders.Length, result.Length);

            for (int i = 0; i < orders.Length; i++)
            {
                Assert.Equal(orders[i].ProductCode, result[i].ProductCode);
                Assert.Equal(orders[i].TotalPrice, result[i].TotalPrice);
                Assert.Equal(orders[i].Quantity, result[i].Quantity);
                Assert.Equal(orders[i].CreatedAt, result[i].CreatedAt);
            }
        }
    }
}
