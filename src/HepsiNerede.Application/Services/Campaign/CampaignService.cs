using HepsiNerede.Application.DTOs.Campaign.CreateCampaign;
using HepsiNerede.Application.DTOs.Campaign.GetActiveCampaignsAndDiscountPercentages;
using HepsiNerede.Application.DTOs.Campaign.GetCampaign;
using HepsiNerede.Application.Services.BaseService;
using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.Domain.Aggregates.CampaignAggregate;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiNerede.Application.Services.Campaign
{
    /// <summary>
    /// Service for handling operations related to campaigns.
    /// </summary>
    public interface ICampaignService : IBaseService
    {
        /// <summary>
        /// Creates a new campaign asynchronously.
        /// </summary>
        /// <param name="createCampaignRequestDTO">The DTO containing campaign creation information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created campaign.</returns>
        Task<Domain.Entities.Campaign> CreateCampaignAsync(CreateCampaignRequestDTO createCampaignRequestDTO);

        /// <summary>
        /// Gets a campaign by name asynchronously.
        /// </summary>
        /// <param name="name">The name of the campaign to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved campaign.</returns>
        Task<Domain.Entities.Campaign?> GetCampaignByNameAsync(string name);

        /// <summary>
        /// Gets the active campaign discount percentage for a product asynchronously.
        /// </summary>
        /// <param name="productCode">The code of the product for which to retrieve the discount percentage.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the discount percentage.</returns>
        Task<decimal> GetActiveCampaignDiscountPercentageForProductAsync(string productCode);

        /// <summary>
        /// Gets active campaigns and their discount percentages asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an array of active campaigns with their discount percentages.</returns>
        Task<GetActiveCampaignsAndDiscountPercentagesDTO[]> GetActiveCampaignsAndDiscountPercentagesAsync();

        /// <summary>
        /// Gets the status of a campaign asynchronously.
        /// </summary>
        /// <param name="campaign">The campaign for which to retrieve the status.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the campaign status.</returns>
        Task<string?> GetCampaignStatusAsync(Domain.Entities.Campaign campaign);

        /// <summary>
        /// Gets the discount percentage for a campaign.
        /// </summary>
        /// <param name="campaign">The campaign for which to retrieve the discount percentage.</param>
        /// <returns>The discount percentage.</returns>
        decimal GetDiscountPercentage(Domain.Entities.Campaign campaign);
    }

    /// <summary>
    /// Implementation of <see cref="ICampaignService"/> for handling operations related to campaigns.
    /// </summary>
    public class CampaignService : BaseService<ICampaignRepository>, ICampaignService
    {
        private readonly ITimeSimulationService _timeSimulationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignService"/> class.
        /// </summary>
        /// <param name="repository">The repository dependency.</param>
        /// <param name="timeSimulationService">The time simulation service dependency.</param>
        public CampaignService(
            ICampaignRepository repository,
            ITimeSimulationService timeSimulationService
         ) : base(repository)
        {
            _timeSimulationService = timeSimulationService;
        }

        /// <inheritdoc/>
        public async Task<Domain.Entities.Campaign?> GetCampaignByNameAsync(string name)
        {
            return await _repository.GetCampaignByNameAsync(name);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<string?> GetCampaignStatusAsync(Domain.Entities.Campaign campaign)
        {
            var currentTime = _timeSimulationService.GetCurrentTime();
            var campaignEndTime = campaign.CreatedAt.AddHours(campaign.Duration);
            if (currentTime > campaignEndTime)
                return Enum.GetName(typeof(CampaignStatus), CampaignStatus.Ended);

            if (currentTime < campaign.CreatedAt)
                return Enum.GetName(typeof(CampaignStatus), CampaignStatus.Incoming);

            return Enum.GetName(typeof(CampaignStatus), CampaignStatus.Active);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<decimal> GetActiveCampaignDiscountPercentageForProductAsync(string productCode)
        {
            var campaign = await _repository.GetActiveCampaignForProductAsync(productCode, _timeSimulationService.GetCurrentTime());
            Debug.WriteLine(_timeSimulationService.GetCurrentTime());
            if (campaign == null)
                return -1;

            return GetDiscountPercentage(campaign);
        }

        /// <inheritdoc/>
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
