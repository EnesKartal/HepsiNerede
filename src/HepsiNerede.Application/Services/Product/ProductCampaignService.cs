using HepsiNerede.Application.Services.Campaign;
using HepsiNerede.Domain.Aggregates.ProductAggregate;

namespace HepsiNerede.Application.Services.Product
{
    public interface IProductCampaignService
    {
        Task<Domain.Entities.Product?> GetProductByCodeAsync(string productCode);
    }
    public class ProductCampaignService : IProductCampaignService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICampaignService _campaignService;
        public ProductCampaignService(IProductRepository productRepository, ICampaignService campaignService)
        {
            _productRepository = productRepository;
            _campaignService = campaignService;
        }

        public async Task<Domain.Entities.Product?> GetProductByCodeAsync(string productCode)
        {
            decimal discountPercentage = await _campaignService.GetActiveCampaignDiscountPercentageForProductAsync(productCode);

            Domain.Entities.Product? product = await _productRepository.GetProductByCodeAsync(productCode);

            if (product != null && discountPercentage > 0)
                product.Price = Math.Round(product.Price - (product.Price * discountPercentage), 4);

            return product;
        }
    }
}
