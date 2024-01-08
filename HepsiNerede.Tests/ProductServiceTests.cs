using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Product.AddProduct;
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

            productService.AddProduct(new AddProductRequestDTO { ProductCode = "P001", Price = 100.0m, Stock = 50 });

            productRepositoryMock.Verify(repo => repo.AddProduct(It.IsAny<Product>()), Times.Once);
        }
    }
}
