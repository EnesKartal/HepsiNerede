using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Campaign.CreateCampaign;

namespace HepsiNerede.Services
{
    public interface ICampaignService
    {
        Campaign CreateCampaign(CreateCampaignRequestDTO createCampaignRequestDTO);
        Campaign? GetCampaignByName(string name);
    }

    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;

        public CampaignService(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public Campaign? GetCampaignByName(string name)
        {
            return _campaignRepository.GetCampaignByName(name);
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
            };

            return _campaignRepository.CreateCampaign(newCampaign);
        }

    }
}
