using HepsiNerede.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Data.Repositories
{
    public interface IProductRepository
    {
        Product? GetProductByCode(string productCode);
        Product CreateProduct(Product product);
        void DecreaseProductStock(string productCode, decimal quantity);
        void DecreaseProductPrice(string productCode, decimal price);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly HepsiNeredeDBContext _dbContext;

        public ProductRepository(HepsiNeredeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Product? GetProductByCode(string productCode)
        {
            return _dbContext.Products
            .AsNoTracking()
            .FirstOrDefault(p => p.ProductCode == productCode);
        }

        public Product CreateProduct(Product product)
        {
            _dbContext.Add(product);
            _dbContext.SaveChanges();
            return product;
        }

        public void DecreaseProductStock(string productCode, decimal quantity)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductCode == productCode);
            product.Stock -= quantity;
            _dbContext.SaveChanges();
        }

        public void DecreaseProductPrice(string productCode, decimal discountPercentage)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.ProductCode == productCode);
            product.Price -= product.Price * (discountPercentage / 100);
            _dbContext.SaveChanges();
        }
    }
}
