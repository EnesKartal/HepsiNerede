using HepsiNerede.Application.DTOs.Product.CreateProduct;
using HepsiNerede.Application.Services.BaseService;
using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.Domain.Aggregates.ProductAggregate;
using System;
using System.Threading.Tasks;

namespace HepsiNerede.Application.Services.Product
{
    /// <summary>
    /// Service for handling product-related operations.
    /// </summary>
    public interface IProductService : IBaseService
    {
        /// <summary>
        /// Creates a new product based on the provided DTO.
        /// </summary>
        /// <param name="createProductDTO">DTO containing information for creating the product.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created product.</returns>
        Task<Domain.Entities.Product> CreateProductAsync(CreateProductRequestDTO createProductDTO);

        /// <summary>
        /// Decreases the stock of the specified product by the given quantity.
        /// </summary>
        /// <param name="productCode">The code of the product whose stock will be decreased.</param>
        /// <param name="quantity">The quantity to decrease from the product stock.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DecreaseProductStockAsync(string productCode, decimal quantity);
    }

    /// <summary>
    /// Implementation of <see cref="IProductService"/> for handling product-related operations.
    /// </summary>
    public class ProductService : BaseService<IProductRepository>, IProductService
    {
        private readonly ITimeSimulationService _timeSimulationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="repository">The repository for product-related data.</param>
        /// <param name="timeSimulationService">The service for time simulation.</param>
        public ProductService(IProductRepository repository, ITimeSimulationService timeSimulationService)
            : base(repository)
        {
            _timeSimulationService = timeSimulationService ?? throw new ArgumentNullException(nameof(timeSimulationService));
        }

        /// <inheritdoc/>
        public async Task<Domain.Entities.Product> CreateProductAsync(CreateProductRequestDTO createProductDTO)
        {
            var newProduct = new Domain.Entities.Product
            {
                ProductCode = createProductDTO.ProductCode,
                Price = createProductDTO.Price,
                Stock = createProductDTO.Stock,
                CreatedAt = _timeSimulationService.GetCurrentTime()
            };

            return await _repository.CreateProductAsync(newProduct);
        }

        /// <inheritdoc/>
        public async Task DecreaseProductStockAsync(string productCode, decimal quantity)
        {
            await _repository.DecreaseProductStockAsync(productCode, quantity);
        }
    }
}
