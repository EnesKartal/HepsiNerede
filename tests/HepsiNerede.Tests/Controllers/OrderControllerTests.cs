using HepsiNerede.Application.DTOs.Order;
using HepsiNerede.Application.Services.Order;
using HepsiNerede.WebApp.Common;
using HepsiNerede.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HepsiNerede.Tests.Controllers
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task CreateProductAsync_WithValidRequest_ShouldReturnOkResult()
        {
            var orderCreationServiceMock = new Mock<IOrderCreationService>();
            var orderController = new OrderController(orderCreationServiceMock.Object);

            var createOrderRequest = new CreateOrderRequestDTO
            {
                ProductCode = "P1",
                Quantity = 100
            };

            var createdOrder = new CreateOrderResponseDTO
            {
                Product = "P1",
                Quantity = 100,
            };

            orderCreationServiceMock.Setup(x => x.CreateOrderAsync(It.IsAny<CreateOrderRequestDTO>())).ReturnsAsync(createdOrder);

            var result = await orderController.CreateProductAsync(createOrderRequest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<CreateOrderResponseDTO>>(okResult.Value);
            Assert.Equal(createdOrder, apiResponse.Data);
        }

        [Fact]
        public async Task CreateProductAsync_WithNullRequest_ShouldReturnBadRequestResult()
        {
            var orderCreationServiceMock = new Mock<IOrderCreationService>();
            var orderController = new OrderController(orderCreationServiceMock.Object);

            var result = await orderController.CreateProductAsync(null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Order is required.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateProductAsync_WithEmptyProductCode_ShouldReturnBadRequestResult()
        {
            var orderCreationServiceMock = new Mock<IOrderCreationService>();
            var orderController = new OrderController(orderCreationServiceMock.Object);

            var createOrderRequest = new CreateOrderRequestDTO
            {
                ProductCode = string.Empty
            };

            var result = await orderController.CreateProductAsync(createOrderRequest);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Product code is required.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateProductAsync_WithZeroQuantity_ShouldReturnBadRequestResult()
        {
            var orderCreationServiceMock = new Mock<IOrderCreationService>();
            var orderController = new OrderController(orderCreationServiceMock.Object);

            var createOrderRequest = new CreateOrderRequestDTO
            {
                Quantity = 0,
                ProductCode = "P1"
            };

            var result = await orderController.CreateProductAsync(createOrderRequest);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Quantity must be greater than 0.", badRequestResult.Value);
        }
    }
}
