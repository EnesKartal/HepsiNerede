public interface ICampaignRepository
{
    Campaign? GetCampaignByName(string name);
    void AddCampaign(Campaign product);
}

public class CampaignRepository : ICampaignRepository
{
    private readonly List<Campaign> _campaigns;

    public CampaignRepository()
    {
        _campaigns = new List<Campaign>();
    }

    public Campaign? GetCampaignByName(string name)
    {
        return _campaigns.FirstOrDefault(p => p.Name == name);
    }

    public void AddCampaign(Campaign campaign)
    {
        _campaigns.Add(campaign);
    }
}
