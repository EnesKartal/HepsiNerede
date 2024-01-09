using HepsiNerede.Domain.Aggregates.CampaignAggregate;
using HepsiNerede.Models.DTO.Campaign.CreateCampaign;
using HepsiNerede.Models.DTO.Product.GetActiveCampaignsAndDiscountPercentages;
using HepsiNerede.Models.DTO.Product.GetProduct;
using System.Diagnostics;

namespace HepsiNerede.Services
{
    public interface ICampaignService
    {
        Task<Campaign> CreateCampaignAsync(CreateCampaignRequestDTO createCampaignRequestDTO);
        Task<GetCampaignResponseDTO?> GetCampaignByNameAsync(string name);
        Task<decimal> GetActiveCampaignDiscountPercentageForProductAsync(string productCode);
        Task<GetActiveCampaignsAndDiscountPercentagesDTO[]> GetActiveCampaignsAndDiscountPercentagesAsync();
        decimal GetDiscountPercentage(Campaign campaign);
    }

    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly ITimeSimulationService _timeSimulationService;
        private readonly IOrderService _orderService;

        public CampaignService(
            ICampaignRepository campaignRepository,
        ITimeSimulationService timeSimulationService,
        IOrderService orderService
         )
        {
            _campaignRepository = campaignRepository;
            _timeSimulationService = timeSimulationService;
            _orderService = orderService;
        }

        public async Task<GetCampaignResponseDTO?> GetCampaignByNameAsync(string name)
        {
            var campaign = await _campaignRepository.GetCampaignByNameAsync(name);
            if (campaign == null)
                return null;

            var productSoldForCampaign = await _orderService.GetOrdersForCampaignProductAsync(campaign.ProductCode, campaign.CreatedAt.AddHours(campaign.Duration));

            return new GetCampaignResponseDTO
            {
                Status = await GetCampaignStatusAsync(name),
                TargetSales = campaign.TargetSalesCount,
                AverageItemPrice = productSoldForCampaign.Length > 0 ? productSoldForCampaign.Average(x => x.TotalPrice / x.Quantity) : 0,
                TotalSales = productSoldForCampaign.Length > 0 ? productSoldForCampaign.Sum(x => x.Quantity) : 0,
                Turnover = productSoldForCampaign.Length > 0 ? productSoldForCampaign.Sum(x => x.TotalPrice) : 0,
            };
        }

        public async Task<Campaign> CreateCampaignAsync(CreateCampaignRequestDTO createCampaignRequestDTO)
        {
            var newCampaign = new Campaign
            {
                Name = createCampaignRequestDTO.Name,
                ProductCode = createCampaignRequestDTO.ProductCode,
                Duration = createCampaignRequestDTO.Duration,
                PriceManipulationLimit = createCampaignRequestDTO.PMLimit,
                TargetSalesCount = createCampaignRequestDTO.TSCount,
                CreatedAt = _timeSimulationService.GetCurrentTime()
            };

            return await _campaignRepository.CreateCampaignAsync(newCampaign);
        }

        private async Task<string?> GetCampaignStatusAsync(string name)
        {
            var campaign = await _campaignRepository.GetCampaignByNameAsync(name);

            if (campaign == null)
                return null;

            var currentTime = _timeSimulationService.GetCurrentTime();
            var campaignEndTime = campaign.CreatedAt.AddHours(campaign.Duration);
            if (currentTime > campaignEndTime)
                return Enum.GetName(CampaignStatus.Ended);

            if (currentTime < campaign.CreatedAt)
                return Enum.GetName(CampaignStatus.Incoming);

            return Enum.GetName(CampaignStatus.Active);
        }

        public async Task<GetActiveCampaignsAndDiscountPercentagesDTO[]> GetActiveCampaignsAndDiscountPercentagesAsync()
        {
            DateTime currentTime = _timeSimulationService.GetCurrentTime();
            Campaign[] activeCampaigns = await _campaignRepository.GetActiveCampaignsAsync(currentTime);

            return activeCampaigns.Select(p => new GetActiveCampaignsAndDiscountPercentagesDTO
            {
                ProductCode = p.ProductCode,
                DiscountPercentage = GetDiscountPercentage(p)
            }).ToArray();
        }

        public async Task<decimal> GetActiveCampaignDiscountPercentageForProductAsync(string productCode)
        {
            var campaign = await _campaignRepository.GetActiveCampaignForProductAsync(productCode, _timeSimulationService.GetCurrentTime());
            Debug.WriteLine(_timeSimulationService.GetCurrentTime());
            if (campaign == null)
                return -1;

            return GetDiscountPercentage(campaign);
        }


        public decimal GetDiscountPercentage(Campaign campaign)
        {
            //TODO: Reduce complexity
            var currentTime = _timeSimulationService.GetCurrentTime();
            var timePassed = currentTime - campaign.CreatedAt;
            decimal percentage = (decimal)(timePassed.TotalHours * 5);
            if (percentage > campaign.PriceManipulationLimit)
                percentage = campaign.PriceManipulationLimit;
            if (percentage < 0)
                percentage = 0;

            return percentage / 100;
        }
    }
}
