using HepsiNerede.Domain.Aggregates.CampaignAggregate;
using HepsiNerede.Domain.Entities;
using HepsiNerede.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Infrastructure.Repositories
{
    public class CampaignRepository : BaseRepository, ICampaignRepository
    {
        public CampaignRepository(HepsiNeredeDBContext dbContext) : base(dbContext) { }

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
