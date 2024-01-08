using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
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
            var orderService = new OrderService(orderRepositoryMock.Object);

            orderService.AddOrder("P001", 5);

            orderRepositoryMock.Verify(repo => repo.AddOrder(It.IsAny<Order>()), Times.Once);
        }
    }
}
