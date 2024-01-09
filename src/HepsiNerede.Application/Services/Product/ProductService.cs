using HepsiNerede.Application.DTOs.Product.CreateProduct;
using HepsiNerede.Application.Services.BaseService;
using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.Domain.Aggregates.ProductAggregate;

namespace HepsiNerede.Application.Services.Product
{
    public interface IProductService : IBaseService
    {
        Task<Domain.Entities.Product> CreateProductAsync(CreateProductRequestDTO createProductDTO);
        Task DecreaseProductStockAsync(string productCode, decimal quantity);
    }

    public class ProductService : BaseService<IProductRepository>, IProductService
    {
        private readonly ITimeSimulationService _timeSimulationService;

        public ProductService(IProductRepository repository,
         ITimeSimulationService timeSimulationService) : base(repository)
        {
            _timeSimulationService = timeSimulationService;
        }

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

        public async Task DecreaseProductStockAsync(string productCode, decimal quantity)
        {
            await _repository.DecreaseProductStockAsync(productCode, quantity);
        }
    }
}
