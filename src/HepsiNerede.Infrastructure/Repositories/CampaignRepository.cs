using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Domain.Aggregates.CampaignAggregate;
using HepsiNerede.Domain.Entities;
using HepsiNerede.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiNerede.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for accessing campaign-related data.
    /// </summary>
    public class CampaignRepository : BaseRepository, ICampaignRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to be used by the repository.</param>
        public CampaignRepository(HepsiNeredeDbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Gets a campaign by its name asynchronously.
        /// </summary>
        /// <param name="name">The name of the campaign to retrieve.</param>
        /// <returns>The campaign with the specified name, or null if not found.</returns>
        public async Task<Campaign?> GetCampaignByNameAsync(string name)
        {
            return await _dbContext.Campaigns
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        /// <summary>
        /// Creates a new campaign asynchronously.
        /// </summary>
        /// <param name="campaign">The campaign to be created.</param>
        /// <returns>The created campaign.</returns>
        public async Task<Campaign> CreateCampaignAsync(Campaign campaign)
        {
            await _dbContext.Campaigns.AddAsync(campaign);
            await _dbContext.SaveChangesAsync();
            return campaign;
        }

        /// <summary>
        /// Gets active campaigns that are still running at the specified simulated date asynchronously.
        /// </summary>
        /// <param name="simulatedDate">The simulated date to check against.</param>
        /// <returns>An array of active campaigns.</returns>
        public async Task<Campaign[]> GetActiveCampaignsAsync(DateTime simulatedDate)
        {
            return await _dbContext.Campaigns
                .AsNoTracking()
                .Where(c => c.CreatedAt.AddHours(c.Duration) > simulatedDate)
                .ToArrayAsync();
        }

        /// <summary>
        /// Gets the active campaign for a product at the specified simulated date asynchronously.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="simulatedDate">The simulated date to check against.</param>
        /// <returns>The active campaign for the product, or null if not found.</returns>
        public async Task<Campaign?> GetActiveCampaignForProductAsync(string productCode, DateTime simulatedDate)
        {
            return await _dbContext.Campaigns
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ProductCode == productCode && c.CreatedAt.AddHours(c.Duration) > simulatedDate);
        }
    }
}
