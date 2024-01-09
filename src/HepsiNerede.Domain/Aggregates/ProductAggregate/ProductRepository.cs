using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Domain.Entities;

namespace HepsiNerede.Domain.Aggregates.ProductAggregate
{
    public interface IProductRepository : IBaseRepository
    {
        Task<Product?> GetProductByCodeAsync(string productCode);
        Task<Product> CreateProductAsync(Product product);
        Task DecreaseProductStockAsync(string productCode, decimal quantity);
    }
}
