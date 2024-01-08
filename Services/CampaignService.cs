public interface ICampaignService
{
    void AddCampaign(string name, string productCode, decimal priceManipulationLimit, decimal targetSaleCount, int duration);
}

public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _campaignRepository;

    public CampaignService(ICampaignRepository campaignRepository)
    {
        _campaignRepository = campaignRepository;
    }

    public void AddCampaign(string name, string productCode, decimal priceManipulationLimit, decimal targetSaleCount, int duration)
    {
        var newCampaign = new Campaign
        {
            Name = name,
            ProductCode = productCode,
            Duration = duration,
            PriceManipulationLimit = priceManipulationLimit,
            TargetSaleCount = targetSaleCount
        };

        _campaignRepository.AddCampaign(newCampaign);
    }
}