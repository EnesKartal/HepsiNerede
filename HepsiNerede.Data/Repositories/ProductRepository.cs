using HepsiNerede.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Data.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByCodeAsync(string productCode);
        Task<Product> CreateProductAsync(Product product);
        Task DecreaseProductStockAsync(string productCode, decimal quantity);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly HepsiNeredeDBContext _dbContext;

        public ProductRepository(HepsiNeredeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetProductByCodeAsync(string productCode)
        {
            return await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductCode == productCode);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task DecreaseProductStockAsync(string productCode, decimal quantity)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == productCode);
            if (product == null)
                throw new Exception($"Product with code {productCode} not found.");

            product.Stock -= quantity;
            await _dbContext.SaveChangesAsync();
        }
    }
}
