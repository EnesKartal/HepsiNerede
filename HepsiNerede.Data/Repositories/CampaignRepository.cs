using HepsiNerede.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Data.Repositories
{
    public interface ICampaignRepository
    {
        Campaign? GetCampaignByName(string name);
        void AddCampaign(Campaign product);
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

        public void AddCampaign(Campaign campaign)
        {
            _dbContext.Campaigns.Add(campaign);
            _dbContext.SaveChanges();
        }
    }
}
