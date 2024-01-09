using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Campaign.CreateCampaign;
using HepsiNerede.Models.DTO.Product.GetActiveCampaignsAndDiscountPercentages;
using HepsiNerede.Models.DTO.Product.GetProduct;
using System.Diagnostics;

namespace HepsiNerede.Services
{
    public interface ICampaignService
    {
        Campaign CreateCampaign(CreateCampaignRequestDTO createCampaignRequestDTO);
        GetCampaignResponseDTO? GetCampaignByName(string name);
        decimal GetActiveCampaignDiscountPercentageForProduct(string productCode);
        GetActiveCampaignsAndDiscountPercentagesDTO[] GetActiveCampaignsAndDiscountPercentages();
        public decimal GetDiscountPercentage(Campaign campaign);
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

        public GetCampaignResponseDTO? GetCampaignByName(string name)
        {
            var campaign = _campaignRepository.GetCampaignByName(name);
            if (campaign == null)
                return null;

            var productSoldForCampaign = _orderService.GetOrdersForCampaignProduct(campaign.ProductCode, campaign.CreatedAt.AddHours(campaign.Duration));

            return new GetCampaignResponseDTO
            {
                Status = GetCampaignStatus(name),
                TargetSales = campaign.TargetSalesCount,
                AverageItemPrice = productSoldForCampaign.Length > 0 ? productSoldForCampaign.Average(x => x.TotalPrice / x.Quantity) : 0,
                TotalSales = productSoldForCampaign.Length > 0 ? productSoldForCampaign.Sum(x => x.Quantity) : 0,
                Turnover = productSoldForCampaign.Length > 0 ? productSoldForCampaign.Sum(x => x.TotalPrice) : 0,
            };
        }

        public Campaign CreateCampaign(CreateCampaignRequestDTO createCampaignRequestDTO)
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

            return _campaignRepository.CreateCampaign(newCampaign);
        }

        private bool GetCampaignStatus(string name)
        {
            var campaign = _campaignRepository.GetCampaignByName(name);

            if (campaign == null)
                return false;

            var currentTime = _timeSimulationService.GetCurrentTime();
            var campaignEndTime = campaign.CreatedAt.AddHours(campaign.Duration);
            return currentTime < campaignEndTime;
        }

        public GetActiveCampaignsAndDiscountPercentagesDTO[] GetActiveCampaignsAndDiscountPercentages()
        {
            DateTime currentTime = _timeSimulationService.GetCurrentTime();
            Campaign[] activeCampaigns = _campaignRepository.GetActiveCampaigns(currentTime);

            return activeCampaigns.Select(p => new GetActiveCampaignsAndDiscountPercentagesDTO
            {
                ProductCode = p.ProductCode,
                DiscountPercentage = GetDiscountPercentage(p)
            }).ToArray();

        }

        public decimal GetActiveCampaignDiscountPercentageForProduct(string productCode)
        {
            var campaign = _campaignRepository.GetActiveCampaignForProduct(productCode, _timeSimulationService.GetCurrentTime());
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
