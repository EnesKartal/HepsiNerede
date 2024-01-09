using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Campaign.CreateCampaign;
using HepsiNerede.Models.DTO.Product.CreateProduct;
using HepsiNerede.Models.DTO.Product.GetActiveCampaignsAndDiscountPercentages;
using HepsiNerede.Models.DTO.Product.GetProduct;
using HepsiNerede.Services;
using HepsiNerede.Tests.Helpers;
using Moq;

namespace HepsiNerede.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void CreateProduct_ShouldReturnProduct()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var productRepository = new ProductRepository(dbContextMock);

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.CreateProduct(It.IsAny<Product>()))
                                 .Returns<Product>(product => productRepository.CreateProduct(product));

            var campaignServiceMock = new Mock<ICampaignService>();
            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var productService = new ProductService(productRepositoryMock.Object, campaignServiceMock.Object, timeSimulationServiceMock.Object);

            var currentTime = timeSimulationServiceMock.Object.GetCurrentTime();

            var createProductRequestDTO = new CreateProductRequestDTO
            {
                ProductCode = "P001",
                Price = 100.0m,
                Stock = 50,
            };

            var createdProduct = productService.CreateProduct(createProductRequestDTO);

            Assert.NotNull(createdProduct);
            Assert.Equal(createProductRequestDTO.ProductCode, createdProduct.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, createdProduct.Price);
            Assert.Equal(createProductRequestDTO.Stock, createdProduct.Stock);
            Assert.Equal(currentTime, createdProduct.CreatedAt);

            productRepositoryMock.Verify(repo => repo.CreateProduct(It.Is<Product>(p => p == createdProduct)), Times.Once);
        }

        [Fact]
        public void GetProduct_ShouldReturnProductResponseDTO()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var productRepository = new ProductRepository(dbContextMock);

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.GetProductByCode(It.IsAny<string>()))
                                 .Returns<string>(productCode => productRepository.GetProductByCode(productCode));

            productRepositoryMock.Setup(x => x.CreateProduct(It.IsAny<Product>()))
                                 .Returns<Product>(product => productRepository.CreateProduct(product));

            var campaignServiceMock = new Mock<ICampaignService>();
            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();
            var productService = new ProductService(productRepositoryMock.Object, campaignServiceMock.Object, timeSimulationServiceMock.Object);

            var currentTime = timeSimulationServiceMock.Object.GetCurrentTime();

            var createProductRequestDTO = new CreateProductRequestDTO
            {
                ProductCode = "P001",
                Price = 100.0m,
                Stock = 50,
            };

            var createdProduct = productService.CreateProduct(createProductRequestDTO);

            var productResult = productRepositoryMock.Object.GetProductByCode(createProductRequestDTO.ProductCode);

            Assert.NotNull(productResult);
            Assert.NotNull(createdProduct);
            Assert.Equal(createProductRequestDTO.ProductCode, productResult.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, productResult.Price);
            Assert.Equal(createProductRequestDTO.Stock, productResult.Stock);
            Assert.Equal(currentTime, productResult.CreatedAt);

            productRepositoryMock.Verify(repo => repo.GetProductByCode(It.Is<string>(p => p == createProductRequestDTO.ProductCode)), Times.Once);
        }

        [Fact]
        public void GetProductWithDiscount_ShouldReturnProductResponseDTO()
        {
            var dbContextMock = DBContextHelper.GetDbContext();
            var productRepository = new ProductRepository(dbContextMock);
            var campaignRepository = new CampaignRepository(dbContextMock);

            var timeSimulationServiceMock = new Mock<ITimeSimulationService>();

            var productRepositoryMock = new Mock<IProductRepository>();

            productRepositoryMock.Setup(x => x.GetProductByCode(It.IsAny<string>()))
                                 .Returns<string>(productCode => productRepository.GetProductByCode(productCode));

            productRepositoryMock.Setup(x => x.CreateProduct(It.IsAny<Product>()))
                                 .Returns<Product>(product => productRepository.CreateProduct(product));

            var campaignServiceMock = new Mock<ICampaignService>();
            campaignServiceMock.Setup(x => x.CreateCampaign(It.IsAny<CreateCampaignRequestDTO>()))
                .Returns<CreateCampaignRequestDTO>(createCampaignDTO =>
                    campaignRepository.CreateCampaign(new Campaign
                    {
                        Name = createCampaignDTO.Name,
                        ProductCode = createCampaignDTO.ProductCode,
                        Duration = createCampaignDTO.Duration,
                        PriceManipulationLimit = createCampaignDTO.PMLimit,
                        TargetSalesCount = createCampaignDTO.TSCount,
                        CreatedAt = timeSimulationServiceMock.Object.GetCurrentTime()
                    })
                );
            campaignServiceMock.Setup(x => x.GetCampaignByName(It.IsAny<string>()))
                .Returns<string>(campaignName =>
                {
                    var campaign = campaignRepository.GetCampaignByName(campaignName);
                    if (campaign == null)
                        return null;
                    return new GetCampaignResponseDTO
                    {
                        TargetSales = campaign.TargetSalesCount
                    };
                }
                );

            campaignServiceMock.Setup(x => x.GetActiveCampaignsAndDiscountPercentages())
                .Returns(() =>
                {
                    var campaigns = campaignRepository.GetActiveCampaigns(timeSimulationServiceMock.Object.GetCurrentTime());
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

            campaignServiceMock.Setup(x => x.GetActiveCampaignDiscountPercentageForProduct(It.IsAny<string>()))
                .Returns<string>(productCode =>
                {
                    var campaign = campaignRepository.GetActiveCampaignForProduct(productCode, timeSimulationServiceMock.Object.GetCurrentTime());
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

            var productService = new ProductService(productRepositoryMock.Object, campaignServiceMock.Object, timeSimulationServiceMock.Object);

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

            var createdProduct = productService.CreateProduct(createProductRequestDTO);
            var createdCampaign = campaignServiceMock.Object.CreateCampaign(createCampaignRequestDTO);

            var campaignResult = campaignServiceMock.Object.GetCampaignByName(createCampaignRequestDTO.Name);
            Assert.NotNull(campaignResult);

            var productResult = productService.GetProductByCode(createProductRequestDTO.ProductCode);

            Assert.NotNull(createdProduct);
            Assert.NotNull(productResult);
            Assert.Equal(createProductRequestDTO.ProductCode, productResult.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, productResult.Price);
            Assert.Equal(createProductRequestDTO.Stock, productResult.Stock);

            timeSimulationServiceMock.Setup(t => t.GetCurrentTime()).Returns(timeSimulationServiceMock.Object.GetCurrentTime().AddHours(3));

            var productResultWithDiscount = productService.GetProductByCode(createProductRequestDTO.ProductCode);

            Assert.NotNull(productResultWithDiscount);
            Assert.Equal(createProductRequestDTO.ProductCode, productResultWithDiscount.ProductCode);
            Assert.NotEqual(createProductRequestDTO.Price, productResultWithDiscount.Price);
            Assert.Equal(createProductRequestDTO.Stock, productResultWithDiscount.Stock);

            timeSimulationServiceMock.Setup(t => t.GetCurrentTime()).Returns(timeSimulationServiceMock.Object.GetCurrentTime().AddHours(7));

            var productResultWithoutDiscount = productService.GetProductByCode(createProductRequestDTO.ProductCode);

            Assert.NotNull(productResultWithoutDiscount);
            Assert.Equal(createProductRequestDTO.ProductCode, productResultWithoutDiscount.ProductCode);
            Assert.Equal(createProductRequestDTO.Price, productResultWithoutDiscount.Price);
            Assert.Equal(createProductRequestDTO.Stock, productResultWithoutDiscount.Stock);
        }
    }
}
