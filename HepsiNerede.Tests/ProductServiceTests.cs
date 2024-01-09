using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Product.CreateProduct;
using HepsiNerede.Services;
using Moq;

namespace HepsiNerede.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void CreateProduct_ShouldCreateProductToRepository()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var campaignServiceMock = new Mock<ICampaignService>();
            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var productService = new ProductService(productRepositoryMock.Object, campaignServiceMock.Object, timeSimulationServiceMock.Object);

            productService.CreateProduct(new CreateProductRequestDTO { ProductCode = "P001", Price = 100.0m, Stock = 50 });

            productRepositoryMock.Verify(repo => repo.CreateProduct(It.IsAny<Product>()), Times.Once);
        }
    }
}
