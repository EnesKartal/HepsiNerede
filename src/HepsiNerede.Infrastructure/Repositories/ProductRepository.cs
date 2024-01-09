using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Domain.Aggregates.ProductAggregate;
using HepsiNerede.Domain.Entities;
using HepsiNerede.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HepsiNerede.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for accessing product-related data.
    /// </summary>
    public class ProductRepository : BaseRepository, IProductRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to be used by the repository.</param>
        public ProductRepository(HepsiNeredeDbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Gets a product by its code asynchronously.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <returns>The product with the specified code.</returns>
        public async Task<Product?> GetProductByCodeAsync(string productCode)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductCode == productCode);
        }

        /// <summary>
        /// Creates a new product asynchronously.
        /// </summary>
        /// <param name="product">The product to be created.</param>
        /// <returns>The created product.</returns>
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Decreases the stock of a product asynchronously.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="quantity">The quantity to decrease the stock by.</param>
        public async Task DecreaseProductStockAsync(string productCode, decimal quantity)
        {
            var product = await _dbContext.Products
                .FirstOrDefaultAsync(p => p.ProductCode == productCode);

            if (product == null)
                throw new Exception($"Product with code {productCode} not found.");

            product.Stock -= quantity;
            await _dbContext.SaveChangesAsync();
        }
    }
}
