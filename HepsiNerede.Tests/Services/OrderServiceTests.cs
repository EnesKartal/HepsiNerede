using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Order;
using HepsiNerede.Services;
using HepsiNerede.Tests.Helpers;
using Moq;

namespace HepsiNerede.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public void CreateOrder_CreatesOrderCorrectly()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var orderRepository = new OrderRepository(dbContextMock); 
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(x => x.CreateOrder(It.IsAny<Order>())).Returns<Order>(orderRepository.CreateOrder);

            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();

            var orderService = new OrderService(orderRepositoryMock.Object, timeSimulationServiceMock.Object);

            var createOrderRequestDTO = new CreateOrderRequestDTO
            {
                ProductCode = "TestProductCode",
                Quantity = 5,
                TotalPrice = 100
            };

            var currentTime = DateTime.Now;

            timeSimulationServiceMock.Setup(t => t.GetCurrentTime()).Returns(currentTime);

            var createdOrder = orderService.CreateOrder(createOrderRequestDTO);

            Assert.NotNull(createdOrder);
            Assert.Equal(createOrderRequestDTO.ProductCode, createdOrder.ProductCode);
            Assert.Equal(createOrderRequestDTO.Quantity, createdOrder.Quantity);
            Assert.Equal(currentTime, createdOrder.CreatedAt);
            Assert.Equal(createOrderRequestDTO.TotalPrice, createdOrder.TotalPrice);

            orderRepositoryMock.Verify(repo => repo.CreateOrder(It.Is<Order>(o => o == createdOrder)), Times.Once);
        }

        [Fact]
        public void GetOrdersForCampaignProduct_ShouldReturnOrders()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();

            var productCode = "P001";
            var campaignEndTime = timeSimulationServiceMock.Object.GetCurrentTime().AddHours(12);
            var orders = new Order[]
            {
            new Order { ProductCode = productCode, TotalPrice = 50.0m, Quantity = 2, CreatedAt = campaignEndTime.AddHours(-1) },
            new Order { ProductCode = productCode, TotalPrice = 30.0m, Quantity = 1, CreatedAt = campaignEndTime.AddMinutes(-30) }
            };

            orderRepositoryMock.Setup(x => x.GetOrdersForCampaignProduct(productCode, campaignEndTime))
                              .Returns(orders);

            var orderService = new OrderService(orderRepositoryMock.Object, timeSimulationServiceMock.Object);

            var result = orderService.GetOrdersForCampaignProduct(productCode, campaignEndTime);

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
