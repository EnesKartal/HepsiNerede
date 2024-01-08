using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Services;
using Moq;

namespace HepsiNerede.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void AddProduct_ShouldAddProductToRepository()
        {
            var productRepositoryMock = new Mock<IProductRepository>();
            var productService = new ProductService(productRepositoryMock.Object);

            productService.AddProduct("P001", 100.0m, 50);

            productRepositoryMock.Verify(repo => repo.AddProduct(It.IsAny<Product>()), Times.Once);
        }
    }
}
