using System;
using System.Threading.Tasks;
using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Domain.Entities;

namespace HepsiNerede.Domain.Aggregates.CampaignAggregate
{
    /// <summary>
    /// Interface for campaign repository operations.
    /// </summary>
    public interface ICampaignRepository : IBaseRepository
    {
        /// <summary>
        /// Gets a campaign by name asynchronously.
        /// </summary>
        /// <param name="name">The name of the campaign to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, containing the retrieved campaign.</returns>
        Task<Campaign?> GetCampaignByNameAsync(string name);

        /// <summary>
        /// Creates a new campaign asynchronously.
        /// </summary>
        /// <param name="campaign">The campaign to be created.</param>
        /// <returns>A task representing the asynchronous operation, containing the created campaign.</returns>
        Task<Campaign> CreateCampaignAsync(Campaign campaign);

        /// <summary>
        /// Gets all active campaigns asynchronously based on the simulated date.
        /// </summary>
        /// <param name="simulatedDate">The simulated date used to determine active campaigns.</param>
        /// <returns>A task representing the asynchronous operation, containing an array of active campaigns.</returns>
        Task<Campaign[]> GetActiveCampaignsAsync(DateTime simulatedDate);

        /// <summary>
        /// Gets the active campaign for a product asynchronously based on the simulated date.
        /// </summary>
        /// <param name="productCode">The product code for which to get the active campaign.</param>
        /// <param name="simulatedDate">The simulated date used to determine active campaigns.</param>
        /// <returns>A task representing the asynchronous operation, containing the active campaign for the product.</returns>
        Task<Campaign?> GetActiveCampaignForProductAsync(string productCode, DateTime simulatedDate);
    }
}
