using HepsiNerede.Application.DTOs.Order;
using HepsiNerede.Application.Services.Order;
using HepsiNerede.Application.Services.Product;
using Moq;

namespace HepsiNerede.Tests.Services
{
    public class OrderCreationServiceTests
    {
        [Fact]
        public async Task CreateOrderAsync_WithValidProduct_ShouldCreateOrderSuccessfully()
        {
            var productCampaignServiceMock = new Mock<IProductCampaignService>();
            var productServiceMock = new Mock<IProductService>();
            var orderServiceMock = new Mock<IOrderService>();

            var orderCreationService = new OrderCreationService(productCampaignServiceMock.Object, orderServiceMock.Object, productServiceMock.Object);

            var createOrderRequest = new CreateOrderRequestDTO
            {
                ProductCode = "P1",
                Quantity = 5
            };

            var product = new Domain.Entities.Product
            {
                ProductCode = "P1",
                Price = 10,
                Stock = 100
            };

            productCampaignServiceMock.Setup(x => x.GetProductByCodeAsync(It.IsAny<string>())).ReturnsAsync(product);
            productServiceMock.Setup(x => x.DecreaseProductStockAsync(It.IsAny<string>(), It.IsAny<decimal>())).Returns(Task.CompletedTask);
            orderServiceMock.Setup(x => x.BeginTransactionAsync()).Returns(Task.CompletedTask);
            orderServiceMock.Setup(x => x.CreateOrderAsync(It.IsAny<CreateOrderDTO>())).ReturnsAsync(new Domain.Entities.Order { ProductCode = "P1", Quantity = 5 });
            orderServiceMock.Setup(x => x.CommitTransactionAsync()).Returns(Task.CompletedTask);

            var result = await orderCreationService.CreateOrderAsync(createOrderRequest);

            Assert.NotNull(result);
            Assert.Equal("P1", result.Product);
            Assert.Equal(5, result.Quantity);

            // Verify that methods were called as expected
            productCampaignServiceMock.Verify(x => x.GetProductByCodeAsync("P1"), Times.Once);
            productServiceMock.Verify(x => x.DecreaseProductStockAsync("P1", 5), Times.Once);
            orderServiceMock.Verify(x => x.BeginTransactionAsync(), Times.Once);
            orderServiceMock.Verify(x => x.CreateOrderAsync(It.IsAny<CreateOrderDTO>()), Times.Once);
            orderServiceMock.Verify(x => x.CommitTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_WithInvalidProduct_ShouldThrowException()
        {
            var productCampaignServiceMock = new Mock<IProductCampaignService>();
            var productServiceMock = new Mock<IProductService>();
            var orderServiceMock = new Mock<IOrderService>();

            var orderCreationService = new OrderCreationService(productCampaignServiceMock.Object, orderServiceMock.Object, productServiceMock.Object);

            var createOrderRequest = new CreateOrderRequestDTO
            {
                ProductCode = "NonExistingProduct",
                Quantity = 5
            };

            productCampaignServiceMock.Setup(x => x.GetProductByCodeAsync(It.IsAny<string>())).ReturnsAsync((Domain.Entities.Product)null); 

            await Assert.ThrowsAsync<Exception>(async () => await orderCreationService.CreateOrderAsync(createOrderRequest));

            productCampaignServiceMock.Verify(x => x.GetProductByCodeAsync("NonExistingProduct"), Times.Once);
            productServiceMock.Verify(x => x.DecreaseProductStockAsync(It.IsAny<string>(), It.IsAny<decimal>()), Times.Never);
            orderServiceMock.Verify(x => x.BeginTransactionAsync(), Times.Never);
            orderServiceMock.Verify(x => x.CreateOrderAsync(It.IsAny<CreateOrderDTO>()), Times.Never);
            orderServiceMock.Verify(x => x.CommitTransactionAsync(), Times.Never);
            orderServiceMock.Verify(x => x.RollbackTransactionAsync(), Times.Never);
        }
    }
}