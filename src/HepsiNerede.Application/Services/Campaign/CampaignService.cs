using HepsiNerede.Application.DTOs.Campaign.CreateCampaign;
using HepsiNerede.Application.DTOs.Campaign.GetActiveCampaignsAndDiscountPercentages;
using HepsiNerede.Application.DTOs.Campaign.GetCampaign;
using HepsiNerede.Application.Services.BaseService;
using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.Domain.Aggregates.CampaignAggregate;
using System.Diagnostics;

namespace HepsiNerede.Application.Services.Campaign
{
    public interface ICampaignService : IBaseService
    {
        Task<Domain.Entities.Campaign> CreateCampaignAsync(CreateCampaignRequestDTO createCampaignRequestDTO);
        Task<Domain.Entities.Campaign?> GetCampaignByNameAsync(string name);
        Task<decimal> GetActiveCampaignDiscountPercentageForProductAsync(string productCode);
        Task<GetActiveCampaignsAndDiscountPercentagesDTO[]> GetActiveCampaignsAndDiscountPercentagesAsync();
        Task<string?> GetCampaignStatusAsync(Domain.Entities.Campaign campaign);
        decimal GetDiscountPercentage(Domain.Entities.Campaign campaign);
    }

    public class CampaignService : BaseService<ICampaignRepository>, ICampaignService
    {
        private readonly ITimeSimulationService _timeSimulationService;

        public CampaignService(
            ICampaignRepository repository,
            ITimeSimulationService timeSimulationService
         ) : base(repository)
        {
            _timeSimulationService = timeSimulationService;
        }

        public async Task<Domain.Entities.Campaign?> GetCampaignByNameAsync(string name)
        {
            return await _repository.GetCampaignByNameAsync(name);
        }

        public async Task<Domain.Entities.Campaign> CreateCampaignAsync(CreateCampaignRequestDTO createCampaignRequestDTO)
        {
            var newCampaign = new Domain.Entities.Campaign
            {
                Name = createCampaignRequestDTO.Name,
                ProductCode = createCampaignRequestDTO.ProductCode,
                Duration = createCampaignRequestDTO.Duration,
                PriceManipulationLimit = createCampaignRequestDTO.PMLimit,
                TargetSalesCount = createCampaignRequestDTO.TSCount,
                CreatedAt = _timeSimulationService.GetCurrentTime()
            };

            return await _repository.CreateCampaignAsync(newCampaign);
        }

        public async Task<string?> GetCampaignStatusAsync(Domain.Entities.Campaign campaign)
        {
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
            Domain.Entities.Campaign[] activeCampaigns = await _repository.GetActiveCampaignsAsync(currentTime);

            return activeCampaigns.Select(p => new GetActiveCampaignsAndDiscountPercentagesDTO
            {
                ProductCode = p.ProductCode,
                DiscountPercentage = GetDiscountPercentage(p)
            }).ToArray();
        }

        public async Task<decimal> GetActiveCampaignDiscountPercentageForProductAsync(string productCode)
        {
            var campaign = await _repository.GetActiveCampaignForProductAsync(productCode, _timeSimulationService.GetCurrentTime());
            Debug.WriteLine(_timeSimulationService.GetCurrentTime());
            if (campaign == null)
                return -1;

            return GetDiscountPercentage(campaign);
        }


        public decimal GetDiscountPercentage(Domain.Entities.Campaign campaign)
        {
            var currentTime = _timeSimulationService.GetCurrentTime();
            var timePassed = currentTime - campaign.CreatedAt;

            decimal percentage = Math.Min((decimal)(timePassed.TotalHours * 5), campaign.PriceManipulationLimit);

            percentage = Math.Max(percentage, 0);

            return percentage / 100;
        }
    }
}
