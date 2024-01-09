using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Campaign.CreateCampaign;
using HepsiNerede.Models.DTO.Product.CreateProduct;
using HepsiNerede.Services;
using HepsiNerede.Tests.Helpers;
using Moq;

namespace HepsiNerede.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void CreateProduct_ShouldReturnProduct()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var productRepository = new ProductRepository(dbContextMock);

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.CreateProduct(It.IsAny<Product>()))
                                 .Returns<Product>(product => productRepository.CreateProduct(product));

            var campaignServiceMock = new Mock<ICampaignService>();
            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var productService = new ProductService(productRepositoryMock.Object, campaignServiceMock.Object, timeSimulationServiceMock.Object);

            var currentTime = timeSimulationServiceMock.Object.GetCurrentTime();

            var createProductRequestDTO = new CreateProductRequestDTO
            {
                ProductCode = "P001",
                Price = 100.0m,
                Stock = 50,
            };

            var createdProduct = productService.CreateProduct(createProductRequestDTO);

            Assert.NotNull(createdProduct);
            Assert.Equal(createProductRequestDTO.ProductCode, createdProduct.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, createdProduct.Price);
            Assert.Equal(createProductRequestDTO.Stock, createdProduct.Stock);
            Assert.Equal(currentTime, createdProduct.CreatedAt);

            productRepositoryMock.Verify(repo => repo.CreateProduct(It.Is<Product>(p=> p == createdProduct)), Times.Once);
        }

        [Fact]
        public void GetProduct_ShouldReturnProductResponseDTO()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var productRepository = new ProductRepository(dbContextMock);

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.GetProductByCode(It.IsAny<string>()))
                                 .Returns<string>(productCode => productRepository.GetProductByCode(productCode));

            productRepositoryMock.Setup(x => x.CreateProduct(It.IsAny<Product>()))
                                 .Returns<Product>(product => productRepository.CreateProduct(product));

            var campaignServiceMock = new Mock<ICampaignService>();
            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var productService = new ProductService(productRepositoryMock.Object, campaignServiceMock.Object, timeSimulationServiceMock.Object);

            var currentTime = timeSimulationServiceMock.Object.GetCurrentTime();

            var createProductRequestDTO = new CreateProductRequestDTO
            {
                ProductCode = "P001",
                Price = 100.0m,
                Stock = 50,
            };

            var createdProduct = productService.CreateProduct(createProductRequestDTO);

            var productResult = productRepositoryMock.Object.GetProductByCode(createProductRequestDTO.ProductCode);

            Assert.NotNull(createdProduct);
            Assert.Equal(createProductRequestDTO.ProductCode, productResult.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, productResult.Price);
            Assert.Equal(createProductRequestDTO.Stock, productResult.Stock);
            Assert.Equal(currentTime, productResult.CreatedAt);

            productRepositoryMock.Verify(repo => repo.GetProductByCode(It.Is<string>(p => p == createProductRequestDTO.ProductCode)), Times.Once);
        }
    }
}
