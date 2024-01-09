using HepsiNerede.Application.DTOs.Product.CreateProduct;
using HepsiNerede.Application.DTOs.Product.GetProduct;
using HepsiNerede.Application.Services.Product;
using HepsiNerede.WebApp.Common;
using HepsiNerede.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HepsiNerede.Tests.Controllers
{
    public class ProductControllerTests
    {
        [Fact]
        public async Task GetProductByCodeAsync_WithValidProductCode_ShouldReturnOkResult()
        {
            var productCampaignServiceMock = new Mock<IProductCampaignService>();
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productCampaignServiceMock.Object, productServiceMock.Object);

            var productCode = "P1";
            var getProductResponse = new Domain.Entities.Product { Price = 10.0m, Stock = 100 };

            productCampaignServiceMock.Setup(x => x.GetProductByCodeAsync(productCode)).ReturnsAsync(getProductResponse);

            var result = await productController.GetProductByCodeAsync(productCode);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<GetProductResponseDTO>>(okResult.Value);
            Assert.Equal(getProductResponse.Price, apiResponse.Data.Price);
            Assert.Equal(getProductResponse.Stock, apiResponse.Data.Stock);
        }

        [Fact]
        public async Task GetProductByCodeAsync_WithNullProductCode_ShouldReturnBadRequestResult()
        {
            var productCampaignServiceMock = new Mock<IProductCampaignService>();
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productCampaignServiceMock.Object, productServiceMock.Object);

            var result = await productController.GetProductByCodeAsync(null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Product code is required.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateProductAsync_WithValidProduct_ShouldReturnOkResult()
        {
            var productCampaignServiceMock = new Mock<IProductCampaignService>();
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productCampaignServiceMock.Object, productServiceMock.Object);

            var createProductRequest = new CreateProductRequestDTO { /* Set request properties */ };
            var createdProduct = new Domain.Entities.Product { Price = 20.0m, ProductCode = "P2", Stock = 50 };

            productServiceMock.Setup(x => x.CreateProductAsync(createProductRequest)).ReturnsAsync(createdProduct);

            var result = await productController.CreateProductAsync(createProductRequest);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse<CreateProductResponseDTO>>(okResult.Value);
            Assert.Equal(createdProduct.Price, apiResponse.Data.Price);
            Assert.Equal(createdProduct.ProductCode, apiResponse.Data.Code);
            Assert.Equal(createdProduct.Stock, apiResponse.Data.Stock);
        }

        [Fact]
        public async Task CreateProductAsync_WithNullProduct_ShouldReturnBadRequestResult()
        {
            var productCampaignServiceMock = new Mock<IProductCampaignService>();
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productCampaignServiceMock.Object, productServiceMock.Object);

            var result = await productController.CreateProductAsync(null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Product is required.", badRequestResult.Value);
        }
    }
}