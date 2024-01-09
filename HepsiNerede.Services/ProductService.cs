using System.Diagnostics;
using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Product.CreateProduct;
using HepsiNerede.Models.DTO.Product.GetActiveCampaignsAndDiscountPercentages;

namespace HepsiNerede.Services
{
    public interface IProductService
    {
        Product? GetProductByCode(string productCode);
        Product CreateProduct(CreateProductRequestDTO createProductDTO);
        void DecreaseProductStock(string productCode, decimal quantity);
        void DecraseProductPricesBulk(GetActiveCampaignsAndDiscountPercentagesDTO[] discounts);
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

        public Product? GetProductByCode(string productCode)
        {
            decimal discountPercentage = _campaignService.GetActiveCampaignDiscountPercentageForProduct(productCode);
            Product? product = _productRepository.GetProductByCode(productCode);
            Debug.WriteLine(discountPercentage);
            System.Console.WriteLine(discountPercentage);
            if (product != null && discountPercentage > 0)
                product.Price = Math.Round(product.Price - (product.Price * discountPercentage), 4);

            return product;
        }

        public Product CreateProduct(CreateProductRequestDTO createProductDTO)
        {
            var newProduct = new Product
            {
                ProductCode = createProductDTO.ProductCode,
                Price = createProductDTO.Price,
                Stock = createProductDTO.Stock,
                CreatedAt = _timeSimulationService.GetCurrentTime()
            };

            return _productRepository.CreateProduct(newProduct);
        }

        public void DecreaseProductStock(string productCode, decimal quantity)
        {
            _productRepository.DecreaseProductStock(productCode, quantity);
        }

        public void DecraseProductPricesBulk(GetActiveCampaignsAndDiscountPercentagesDTO[] discounts)
        {
            //TODO: Performance can be improved by using bulk update
            foreach (var discount in discounts)
                DecreaseProductPrice(discount.ProductCode, discount.DiscountPercentage);
        }

        private void DecreaseProductPrice(string productCode, decimal discountPercentage)
        {
            _productRepository.DecreaseProductPrice(productCode, discountPercentage);
        }
    }
}
