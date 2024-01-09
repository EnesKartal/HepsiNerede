using HepsiNerede.Application.Services.Campaign;
using HepsiNerede.Domain.Aggregates.ProductAggregate;
using System;
using System.Threading.Tasks;

namespace HepsiNerede.Application.Services.Product
{
    /// <summary>
    /// Service for handling product-related operations considering active campaigns.
    /// </summary>
    public interface IProductCampaignService
    {
        /// <summary>
        /// Gets a product by its code, considering active campaigns for potential discounts.
        /// </summary>
        /// <param name="productCode">The code of the product to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the product with potential discounts applied.</returns>
        Task<Domain.Entities.Product?> GetProductByCodeAsync(string productCode);
    }

    /// <summary>
    /// Implementation of <see cref="IProductCampaignService"/> for handling product-related operations considering active campaigns.
    /// </summary>
    public class ProductCampaignService : IProductCampaignService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICampaignService _campaignService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCampaignService"/> class.
        /// </summary>
        /// <param name="productRepository">The repository for product-related data.</param>
        /// <param name="campaignService">The service for handling campaigns.</param>
        public ProductCampaignService(IProductRepository productRepository, ICampaignService campaignService)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _campaignService = campaignService ?? throw new ArgumentNullException(nameof(campaignService));
        }

        /// <inheritdoc/>
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
