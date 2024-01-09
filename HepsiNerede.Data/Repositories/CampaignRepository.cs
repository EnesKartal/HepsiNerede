using HepsiNerede.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Data.Repositories
{
    public interface ICampaignRepository
    {
        Campaign? GetCampaignByName(string name);
        Campaign CreateCampaign(Campaign product);
        Campaign[] GetActiveCampaigns(DateTime simulatedDate);
        Campaign GetActiveCampaignForProduct(string productCode, DateTime simulatedDate);
    }

    public class CampaignRepository : ICampaignRepository
    {
        private readonly HepsiNeredeDBContext _dbContext;

        public CampaignRepository(HepsiNeredeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Campaign? GetCampaignByName(string name)
        {
            return _dbContext.Campaigns
            .AsNoTracking()
            .FirstOrDefault(p => p.Name == name);
        }

        public Campaign CreateCampaign(Campaign campaign)
        {
            _dbContext.Campaigns.Add(campaign);
            _dbContext.SaveChanges();
            return campaign;
        }

        public Campaign[] GetActiveCampaigns(DateTime simulatedDate)
        {
            return _dbContext.Campaigns
            .AsNoTracking()
            .Where(c => c.CreatedAt.AddHours(c.Duration) > simulatedDate)
            .ToArray();
        }

        public Campaign GetActiveCampaignForProduct(string productCode, DateTime simulatedDate)
        {
            return _dbContext.Campaigns
            .AsNoTracking()
            .FirstOrDefault(c => c.ProductCode == productCode && c.CreatedAt.AddHours(c.Duration) > simulatedDate);
        }
    }
}
