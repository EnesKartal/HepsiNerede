using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Product.CreateProduct;

namespace HepsiNerede.Services
{
    public interface IProductService
    {
        Task<Product?> GetProductByCodeAsync(string productCode);
        Task<Product> CreateProductAsync(CreateProductRequestDTO createProductDTO);
        Task DecreaseProductStockAsync(string productCode, decimal quantity);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICampaignService _campaignService;
        private readonly ITimeSimulationService _timeSimulationService;

        public ProductService(IProductRepository productRepository,
         ICampaignService campaignService,
         ITimeSimulationService timeSimulationService)
        {
            _productRepository = productRepository;
            _campaignService = campaignService;
            _timeSimulationService = timeSimulationService;
        }

        public async Task<Product?> GetProductByCodeAsync(string productCode)
        {
            decimal discountPercentage = await _campaignService.GetActiveCampaignDiscountPercentageForProductAsync(productCode);

            Product? product = await _productRepository.GetProductByCodeAsync(productCode);

            if (product != null && discountPercentage > 0)
                product.Price = Math.Round(product.Price - (product.Price * discountPercentage), 4);

            return product;
        }

        public async Task<Product> CreateProductAsync(CreateProductRequestDTO createProductDTO)
        {
            var newProduct = new Product
            {
                ProductCode = createProductDTO.ProductCode,
                Price = createProductDTO.Price,
                Stock = createProductDTO.Stock,
                CreatedAt = _timeSimulationService.GetCurrentTime()
            };

            return await _productRepository.CreateProductAsync(newProduct);
        }

        public async Task DecreaseProductStockAsync(string productCode, decimal quantity)
        {
            await _productRepository.DecreaseProductStockAsync(productCode, quantity);
        }
    }
}
