using HepsiNerede.Application.DTOs.Campaign.GetCampaign;
using HepsiNerede.Application.Services.Order;

namespace HepsiNerede.Application.Services.Campaign
{
    public interface ICampaignOrderService
    {
        Task<GetCampaignResponseDTO?> GetCampaignByNameAsync(string name);
    }

    public class CampaignOrderService : ICampaignOrderService
    {
        private readonly ICampaignService _campaignService;
        private readonly IOrderService _orderService;

        public CampaignOrderService(ICampaignService campaignService, IOrderService orderService)
        {
            _campaignService = campaignService;
            _orderService = orderService;
        }

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
