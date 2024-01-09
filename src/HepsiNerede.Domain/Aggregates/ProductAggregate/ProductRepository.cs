using System.Threading.Tasks;
using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Domain.Entities;

namespace HepsiNerede.Domain.Aggregates.ProductAggregate
{
    /// <summary>
    /// Interface for product repository operations.
    /// </summary>
    public interface IProductRepository : IBaseRepository
    {
        /// <summary>
        /// Gets a product by its code asynchronously.
        /// </summary>
        /// <param name="productCode">The code of the product to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, containing the retrieved product or null if not found.</returns>
        Task<Product?> GetProductByCodeAsync(string productCode);

        /// <summary>
        /// Creates a new product asynchronously.
        /// </summary>
        /// <param name="product">The product to be created.</param>
        /// <returns>A task representing the asynchronous operation, containing the created product.</returns>
        Task<Product> CreateProductAsync(Product product);

        /// <summary>
        /// Decreases the stock of a product asynchronously.
        /// </summary>
        /// <param name="productCode">The code of the product to decrease stock.</param>
        /// <param name="quantity">The quantity by which to decrease the stock.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DecreaseProductStockAsync(string productCode, decimal quantity);
    }
}
