using HepsiNerede.Application.DTOs.Campaign.CreateCampaign;
using HepsiNerede.Application.DTOs.Product.CreateProduct;
using HepsiNerede.Application.Services.Campaign;
using HepsiNerede.Application.Services.Order;
using HepsiNerede.Application.Services.Product;
using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.Infrastructure.Repositories;
using HepsiNerede.Tests.Helpers;
using Xunit.Abstractions;

namespace HepsiNerede.Tests.Services
{
    public class AssignmentTests
    {
        private readonly ITestOutputHelper output;
        private readonly ProductService productService;
        private readonly ProductCampaignService productCampaignService;
        private readonly CampaignService campaignService;
        private readonly CampaignOrderService campaignOrderService;
        private readonly TimeSimulationService timeSimulationService;

        private Domain.Entities.Product createdProduct;
        private Domain.Entities.Campaign createdCampaign;
        public AssignmentTests(ITestOutputHelper output)
        {
            this.output = output;
            var dbContextMock = DBContextHelper.GetDbContext();
            var productRepository = new ProductRepository(dbContextMock);
            var campaignRepository = new CampaignRepository(dbContextMock);
            timeSimulationService = new TimeSimulationService();
            var orderService = new OrderService(new OrderRepository(dbContextMock), timeSimulationService);

            campaignService = new CampaignService(campaignRepository, timeSimulationService);
            campaignOrderService = new CampaignOrderService(campaignService, orderService);
            productService = new ProductService(productRepository, timeSimulationService);
            productCampaignService = new ProductCampaignService(productRepository, campaignService);
        }

        [Fact]
        public async void StartAssignmentTest()
        {
            await CreateProductAsync();

            await CreateCampaign();

            await GetProductInfo();

            IncreaseTime(1);

            await GetProductInfo();

            IncreaseTime(1);

            await GetProductInfo();

            IncreaseTime(1);

            await GetProductInfo();

            IncreaseTime(1);

            await GetProductInfo();

            IncreaseTime(2);

            await GetProductInfo();

            await GetCampaignInfo();
        }

        private async Task CreateProductAsync()
        {
            var currentTime = timeSimulationService.GetCurrentTime();

            var createProductRequestDTO = new CreateProductRequestDTO
            {
                ProductCode = "P1",
                Price = 100,
                Stock = 1000,
            };

            createdProduct = await productService.CreateProductAsync(createProductRequestDTO);

            Assert.NotNull(createdProduct);
            Assert.Equal(createProductRequestDTO.ProductCode, createdProduct.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, createdProduct.Price);
            Assert.Equal(createProductRequestDTO.Stock, createdProduct.Stock);
            Assert.Equal(currentTime, createdProduct.CreatedAt);

            output.WriteLine($"Product created; code {createdProduct.ProductCode}, price {createdProduct.Price}, stock {createdProduct.Stock}");
        }

        private async Task CreateCampaign()
        {
            var createCampaignRequestDTO = new CreateCampaignRequestDTO
            {
                Name = "C1",
                ProductCode = "P1",
                Duration = 5,
                PMLimit = 20,
                TSCount = 100
            };

            createdCampaign = await campaignService.CreateCampaignAsync(createCampaignRequestDTO);

            Assert.NotNull(createdCampaign);
            Assert.Equal(createCampaignRequestDTO.Name, createdCampaign.Name);
            Assert.Equal(createCampaignRequestDTO.ProductCode, createdCampaign.ProductCode);
            Assert.Equal(createCampaignRequestDTO.Duration, createdCampaign.Duration);
            Assert.Equal(createCampaignRequestDTO.PMLimit, createdCampaign.PriceManipulationLimit);
            Assert.Equal(createCampaignRequestDTO.TSCount, createdCampaign.TargetSalesCount);
            Assert.Equal(createdProduct.CreatedAt, createdCampaign.CreatedAt);

            output.WriteLine($"Campaign created; name {createdCampaign.Name}, product {createdCampaign.ProductCode}, duration {createdCampaign.Duration}, limit {createdCampaign.PriceManipulationLimit}, target sales count {createdCampaign.TargetSalesCount}");
        }

        private async Task GetProductInfo()
        {
            var product = await productCampaignService.GetProductByCodeAsync(createdProduct.ProductCode);
            var currentCampaign = await campaignService.GetActiveCampaignDiscountPercentageForProductAsync(product.ProductCode);

            decimal priceForAssert = currentCampaign > 0 ? Math.Round(createdProduct.Price - createdProduct.Price * currentCampaign, 4) : createdProduct.Price;

            Assert.NotNull(product);
            Assert.Equal(createdProduct.ProductCode, product.ProductCode);
            Assert.Equal(priceForAssert, product.Price);
            Assert.Equal(createdProduct.Stock, product.Stock);
            Assert.Equal(createdProduct.CreatedAt, product.CreatedAt);

            output.WriteLine($"Product {product.ProductCode} info; price {product.Price}, stock {product.Stock}");
        }

        private async Task GetCampaignInfo()
        {
            var campaign = await campaignOrderService.GetCampaignByNameAsync(createdCampaign.Name);

            Assert.NotNull(campaign);

            output.WriteLine($"Campaign {createdCampaign.Name} info; Status {campaign.Status}, Target Sales {campaign.TargetSales}, Total Sales {campaign.TotalSales}, Turnover {campaign.Turnover}, Average Item Price {campaign.AverageItemPrice}");
        }

        private void IncreaseTime(int hour)
        {
            var newTime = timeSimulationService.IncreaseTime(hour);

            output.WriteLine($"Time is {newTime.ToShortTimeString()}");
        }
    }
}