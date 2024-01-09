using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Order;
using HepsiNerede.Services;
using Moq;

namespace HepsiNerede.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public void AddOrder_ShouldAddOrderToRepository()
        {
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var orderService = new OrderService(orderRepositoryMock.Object, timeSimulationServiceMock.Object);

            orderService.CreateOrder(new CreateOrderRequestDTO { ProductCode = "P001", Quantity = 5 });

            orderRepositoryMock.Verify(repo => repo.CreateOrder(It.IsAny<Order>()), Times.Once);
        }
    }
}
