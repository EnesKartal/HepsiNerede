using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Domain.Entities;

namespace HepsiNerede.Domain.Aggregates.CampaignAggregate
{
    public interface ICampaignRepository : IBaseRepository
    {
        Task<Campaign?> GetCampaignByNameAsync(string name);
        Task<Campaign> CreateCampaignAsync(Campaign product);
        Task<Campaign[]> GetActiveCampaignsAsync(DateTime simulatedDate);
        Task<Campaign?> GetActiveCampaignForProductAsync(string productCode, DateTime simulatedDate);
    }
}
