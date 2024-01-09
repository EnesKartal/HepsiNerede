using HepsiNerede.Application.DTOs.Campaign.GetCampaign;
using HepsiNerede.Application.Services.Order;

namespace HepsiNerede.Application.Services.Campaign
{
    /// <summary>
    /// Service for handling operations related to campaigns and orders.
    /// </summary>
    public interface ICampaignOrderService
    {
        /// <summary>
        /// Gets campaign details by name asynchronously.
        /// </summary>
        /// <param name="name">The name of the campaign to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the campaign details.</returns>
        Task<GetCampaignResponseDTO?> GetCampaignByNameAsync(string name);
    }

    /// <summary>
    /// Implementation of <see cref="ICampaignOrderService"/> for handling operations related to campaigns and orders.
    /// </summary>
    public class CampaignOrderService : ICampaignOrderService
    {
        private readonly ICampaignService _campaignService;
        private readonly IOrderService _orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignOrderService"/> class.
        /// </summary>
        /// <param name="campaignService">The campaign service dependency.</param>
        /// <param name="orderService">The order service dependency.</param>
        public CampaignOrderService(ICampaignService campaignService, IOrderService orderService)
        {
            _campaignService = campaignService;
            _orderService = orderService;
        }

        /// <summary>
        /// Gets campaign details by name asynchronously.
        /// </summary>
        /// <param name="name">The name of the campaign to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the campaign details.</returns>
        public async Task<GetCampaignResponseDTO?> GetCampaignByNameAsync(string name)
        {
            var campaign = await _campaignService.GetCampaignByNameAsync(name);
            if (campaign == null)
                return null;

            var productSoldForCampaign = await _orderService.GetOrdersForCampaignProductAsync(campaign.ProductCode, campaign.CreatedAt, campaign.CreatedAt.AddHours(campaign.Duration));

            return new GetCampaignResponseDTO
            {
                Status = await _campaignService.GetCampaignStatusAsync(campaign),
                TargetSales = campaign.TargetSalesCount,
                AverageItemPrice = productSoldForCampaign.Length > 0 ? productSoldForCampaign.Average(x => x.TotalPrice / x.Quantity) : 0,
                TotalSales = productSoldForCampaign.Length > 0 ? productSoldForCampaign.Sum(x => x.Quantity) : 0,
                Turnover = productSoldForCampaign.Length > 0 ? productSoldForCampaign.Sum(x => x.TotalPrice) : 0,
            };
        }
    }
}
