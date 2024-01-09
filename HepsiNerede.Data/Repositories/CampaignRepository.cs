using HepsiNerede.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Data.Repositories
{
    public interface ICampaignRepository
    {
        Task<Campaign?> GetCampaignByNameAsync(string name);
        Task<Campaign> CreateCampaignAsync(Campaign product);
        Task<Campaign[]> GetActiveCampaignsAsync(DateTime simulatedDate);
        Task<Campaign?> GetActiveCampaignForProductAsync(string productCode, DateTime simulatedDate);
    }

    public class CampaignRepository : ICampaignRepository
    {
        private readonly HepsiNeredeDBContext _dbContext;

        public CampaignRepository(HepsiNeredeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Campaign?> GetCampaignByNameAsync(string name)
        {
            return await _dbContext.Campaigns
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<Campaign> CreateCampaignAsync(Campaign campaign)
        {
            await _dbContext.Campaigns.AddAsync(campaign);
            await _dbContext.SaveChangesAsync();
            return campaign;
        }

        public async Task<Campaign[]> GetActiveCampaignsAsync(DateTime simulatedDate)
        {
            return await _dbContext.Campaigns
            .AsNoTracking()
            .Where(c => c.CreatedAt.AddHours(c.Duration) > simulatedDate)
            .ToArrayAsync();
        }

        public async Task<Campaign?> GetActiveCampaignForProductAsync(string productCode, DateTime simulatedDate)
        {
            return await _dbContext.Campaigns
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ProductCode == productCode && c.CreatedAt.AddHours(c.Duration) > simulatedDate);
        }
    }
}
