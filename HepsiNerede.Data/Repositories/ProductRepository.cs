using HepsiNerede.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Data.Repositories
{
    public interface IProductRepository
    {
        Product? GetProductByCode(string productCode);
        void AddProduct(Product product);
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

        public void AddProduct(Product product)
        {
            _dbContext.Add(product);
            _dbContext.SaveChanges();
        }
    }
}
