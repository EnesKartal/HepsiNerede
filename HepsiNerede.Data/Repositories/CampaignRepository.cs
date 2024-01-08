using HepsiNerede.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Data.Repositories
{
    public interface ICampaignRepository
    {
        Campaign? GetCampaignByName(string name);
        Campaign CreateCampaign(Campaign product);
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
            campaign.CreatedAt = DateTime.Now;
            _dbContext.Campaigns.Add(campaign);
            _dbContext.SaveChanges();
            return campaign;
        }
    }
}
