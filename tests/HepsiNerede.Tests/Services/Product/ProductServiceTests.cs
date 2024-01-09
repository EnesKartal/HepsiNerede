using HepsiNerede.Application.DTOs.Campaign.CreateCampaign;
using HepsiNerede.Application.DTOs.Campaign.GetActiveCampaignsAndDiscountPercentages;
using HepsiNerede.Application.DTOs.Campaign.GetCampaign;
using HepsiNerede.Application.DTOs.Product.CreateProduct;
using HepsiNerede.Application.Services.Campaign;
using HepsiNerede.Application.Services.Order;
using HepsiNerede.Application.Services.Product;
using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.Domain.Aggregates.ProductAggregate;
using HepsiNerede.Domain.Entities;
using HepsiNerede.Infrastructure.Repositories;
using HepsiNerede.Tests.Helpers;
using Moq;

namespace HepsiNerede.Tests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public async void CreateProduct_ShouldReturnProduct()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var productRepository = new ProductRepository(dbContextMock);

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>()))
                                 .Returns<Product>(product => productRepository.CreateProductAsync(product));

            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var productService = new ProductService(productRepositoryMock.Object, timeSimulationServiceMock.Object);

            var currentTime = timeSimulationServiceMock.Object.GetCurrentTime();

            var createProductRequestDTO = new CreateProductRequestDTO
            {
                ProductCode = "P001",
                Price = 100.0m,
                Stock = 50,
            };

            var createdProduct = await productService.CreateProductAsync(createProductRequestDTO);

            Assert.NotNull(createdProduct);
            Assert.Equal(createProductRequestDTO.ProductCode, createdProduct.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, createdProduct.Price);
            Assert.Equal(createProductRequestDTO.Stock, createdProduct.Stock);
            Assert.Equal(currentTime, createdProduct.CreatedAt);

            productRepositoryMock.Verify(repo => repo.CreateProductAsync(It.Is<Product>(p => p == createdProduct)), Times.Once);
        }

        [Fact]
        public async void GetProduct_ShouldReturnProductResponseDTO()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var productRepository = new ProductRepository(dbContextMock);

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.GetProductByCodeAsync(It.IsAny<string>()))
                                 .Returns<string>(productCode => productRepository.GetProductByCodeAsync(productCode));

            productRepositoryMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>()))
                                 .Returns<Product>(product => productRepository.CreateProductAsync(product));

            var campaignServiceMock = new Mock<ICampaignService>();
            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var productService = new ProductService(productRepositoryMock.Object, timeSimulationServiceMock.Object);

            var currentTime = timeSimulationServiceMock.Object.GetCurrentTime();

            var createProductRequestDTO = new CreateProductRequestDTO
            {
                ProductCode = "P001",
                Price = 100.0m,
                Stock = 50,
            };

            var createdProduct = await productService.CreateProductAsync(createProductRequestDTO);

            var productResult = await productRepositoryMock.Object.GetProductByCodeAsync(createProductRequestDTO.ProductCode);

            Assert.NotNull(productResult);
            Assert.NotNull(createdProduct);
            Assert.Equal(createProductRequestDTO.ProductCode, productResult.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, productResult.Price);
            Assert.Equal(createProductRequestDTO.Stock, productResult.Stock);
            Assert.Equal(currentTime, productResult.CreatedAt);

            productRepositoryMock.Verify(repo => repo.GetProductByCodeAsync(It.Is<string>(p => p == createProductRequestDTO.ProductCode)), Times.Once);
        }

        [Fact]
        public async void GetProductWithDiscount_ShouldReturnProductResponseDTO()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var productRepository = new ProductRepository(dbContextMock);
            var campaignRepository = new CampaignRepository(dbContextMock);

            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();

            var productRepositoryMock = new Mock<IProductRepository>();

            productRepositoryMock.Setup(x => x.GetProductByCodeAsync(It.IsAny<string>()))
                                 .Returns<string>(productCode => productRepository.GetProductByCodeAsync(productCode));

            productRepositoryMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>()))
                                 .Returns<Product>(product => productRepository.CreateProductAsync(product));

            var campaignServiceMock = new Mock<ICampaignService>();
            var campaignOrderServiceMock = new Mock<ICampaignOrderService>();

            campaignServiceMock.Setup(x => x.CreateCampaignAsync(It.IsAny<CreateCampaignRequestDTO>()))
                .Returns<CreateCampaignRequestDTO>(createCampaignDTO =>
                    campaignRepository.CreateCampaignAsync(new Campaign
                    {
                        Name = createCampaignDTO.Name,
                        ProductCode = createCampaignDTO.ProductCode,
                        Duration = createCampaignDTO.Duration,
                        PriceManipulationLimit = createCampaignDTO.PMLimit,
                        TargetSalesCount = createCampaignDTO.TSCount,
                        CreatedAt = timeSimulationServiceMock.Object.GetCurrentTime()
                    })
                );

            campaignOrderServiceMock.Setup(x => x.GetCampaignByNameAsync(It.IsAny<string>()))
                .Returns<string>(async campaignName =>
                {
                    var campaign = await campaignRepository.GetCampaignByNameAsync(campaignName);
                    if (campaign == null)
                        return null;

                    return new GetCampaignResponseDTO
                    {
                        TargetSales = campaign.TargetSalesCount
                    };
                });

            campaignServiceMock.Setup(x => x.GetActiveCampaignsAndDiscountPercentagesAsync())
                .Returns(async () =>
                {
                    var campaigns = await campaignRepository.GetActiveCampaignsAsync(timeSimulationServiceMock.Object.GetCurrentTime());
                    var campaignDiscountPercentages = new GetActiveCampaignsAndDiscountPercentagesDTO[campaigns.Length];
                    for (int i = 0; i < campaigns.Length; i++)
                    {
                        campaignDiscountPercentages[i] = new GetActiveCampaignsAndDiscountPercentagesDTO
                        {
                            ProductCode = campaigns[i].ProductCode,
                            DiscountPercentage = campaigns[i].PriceManipulationLimit
                        };
                    }
                    return campaignDiscountPercentages;
                });

            campaignServiceMock.Setup(x => x.GetActiveCampaignDiscountPercentageForProductAsync(It.IsAny<string>()))
                .Returns<string>(async productCode =>
                {
                    var campaign = await campaignRepository.GetActiveCampaignForProductAsync(productCode, timeSimulationServiceMock.Object.GetCurrentTime());
                    if (campaign == null)
                        return 0;
                    return campaignServiceMock.Object.GetDiscountPercentage(campaign);
                });

            campaignServiceMock.Setup(x => x.GetDiscountPercentage(It.IsAny<Campaign>()))
                .Returns<Campaign>(campaign =>
                {
                    if (campaign == null)
                        return 0;
                    var currentTime = timeSimulationServiceMock.Object.GetCurrentTime();
                    var timePassed = currentTime - campaign.CreatedAt;
                    decimal percentage = (decimal)(timePassed.TotalHours * 5);
                    if (percentage > campaign.PriceManipulationLimit)
                        percentage = campaign.PriceManipulationLimit;
                    if (percentage < 0)
                        percentage = 0;

                    return percentage / 100;
                });

            var orderServiceMock = new Mock<IOrderService>();

            var productService = new ProductService(productRepositoryMock.Object, timeSimulationServiceMock.Object);
            var productCampaignService = new ProductCampaignService(productRepositoryMock.Object, campaignServiceMock.Object);

            var createProductRequestDTO = new CreateProductRequestDTO
            {
                ProductCode = "P001",
                Price = 100.0m,
                Stock = 50,
            };

            var createCampaignRequestDTO = new CreateCampaignRequestDTO
            {
                Name = "TestCampaign",
                ProductCode = "P001",
                Duration = 5,
                PMLimit = 20,
                TSCount = 100
            };

            var createdProduct = await productService.CreateProductAsync(createProductRequestDTO);
            var createdCampaign = await campaignServiceMock.Object.CreateCampaignAsync(createCampaignRequestDTO);

            var campaignResult = await campaignOrderServiceMock.Object.GetCampaignByNameAsync(createCampaignRequestDTO.Name);
            Assert.NotNull(campaignResult);

            var productResult = await productCampaignService.GetProductByCodeAsync(createProductRequestDTO.ProductCode);

            Assert.NotNull(createdProduct);
            Assert.NotNull(productResult);
            Assert.Equal(createProductRequestDTO.ProductCode, productResult.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, productResult.Price);
            Assert.Equal(createProductRequestDTO.Stock, productResult.Stock);

            timeSimulationServiceMock.Setup(t => t.GetCurrentTime()).Returns(timeSimulationServiceMock.Object.GetCurrentTime().AddHours(3));

            var productResultWithDiscount = await productCampaignService.GetProductByCodeAsync(createProductRequestDTO.ProductCode);

            Assert.NotNull(productResultWithDiscount);
            Assert.Equal(createProductRequestDTO.ProductCode, productResultWithDiscount.ProductCode);
            Assert.NotEqual(createProductRequestDTO.Price, productResultWithDiscount.Price);
            Assert.Equal(createProductRequestDTO.Stock, productResultWithDiscount.Stock);

            timeSimulationServiceMock.Setup(t => t.GetCurrentTime()).Returns(timeSimulationServiceMock.Object.GetCurrentTime().AddHours(7));

            var productResultWithoutDiscount = await productCampaignService.GetProductByCodeAsync(createProductRequestDTO.ProductCode);

            Assert.NotNull(productResultWithoutDiscount);
            Assert.Equal(createProductRequestDTO.ProductCode, productResultWithoutDiscount.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, productResultWithoutDiscount.Price);
            Assert.Equal(createProductRequestDTO.Stock, productResultWithoutDiscount.Stock);
        }
    }
}
