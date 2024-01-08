using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Services;
using Moq;

public class CampaignServiceTests
{
    [Fact]
    public void AddCampaign_ShouldAddCampaignToRepository()
    {
        var campaignRepositoryMock = new Mock<ICampaignRepository>();
        var campaignService = new CampaignService(campaignRepositoryMock.Object);

        var campaign = new Campaign
        {
            Name = "SummerSale",
            ProductCode = "P001",
            Duration = 24,
            PriceManipulationLimit = 10,
            TargetSalesCount = 50
        };

        campaignService.AddCampaign(campaign.Name, campaign.ProductCode, campaign.PriceManipulationLimit, campaign.TargetSalesCount, campaign.Duration);

        campaignRepositoryMock.Verify(repo => repo.AddCampaign(campaign), Times.Once);
    }
}
