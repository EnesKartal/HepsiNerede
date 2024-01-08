using HepsiNerede.Data;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Services;
using Microsoft.EntityFrameworkCore;

public class ProductServiceTests
{
    [Fact]
    public void AddProduct_ShouldAddProductToRepository()
    {
        // Arrange
        var dbContext = TestHelper.GetDbContext();
        var productRepository = new ProductRepository(dbContext);
        var productService = new ProductService(productRepository);

        // Act
        productService.AddProduct("P001", 100.0m, 50);

        // Assert
        var product = productRepository.GetProductByCode("P001");
        Assert.NotNull(product);
        Assert.Equal("P001", product.ProductCode);
        Assert.Equal(100.0m, product.Price);
        Assert.Equal(50, product.Stock);
    }
}

public static class TestHelper
{
    public static HepsiNeredeDBContext GetDbContext()
    {
        // SQLite In-Memory veritabanını kullanarak bir test bağlamı oluşturun
        var options = new DbContextOptionsBuilder<HepsiNeredeDBContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        var dbContext = new HepsiNeredeDBContext(options);
        dbContext.Database.OpenConnection();
        dbContext.Database.EnsureCreated();
        return dbContext;
    }
}
