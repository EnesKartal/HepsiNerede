using HepsiNerede.Application.DTOs.Campaign.CreateCampaign;
using HepsiNerede.Application.DTOs.Campaign.GetCampaign;
using HepsiNerede.Application.Services.Campaign;
using HepsiNerede.WebApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace HepsiNerede.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly ICampaignOrderService _campaignOrderService;

        public CampaignController(ICampaignService campaignService, ICampaignOrderService campaignOrderService)
        {
            _campaignService = campaignService;
            _campaignOrderService = campaignOrderService;
        }

        [HttpGet("getCampaignByName")]
        public async Task<ActionResult<ApiResponse<GetCampaignResponseDTO>>> GetCampaignByNameAsync([FromQuery] string? name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Campaign name is required.");

            var campaign = await _campaignOrderService.GetCampaignByNameAsync(name);

            if (campaign == null)
                return NotFound($"Campaign with name '{name}' not found.");

            return Ok(new ApiResponse<GetCampaignResponseDTO>(campaign, message: $"Campaign {name} info"));
        }

        /// <summary>
        /// Note: This endpoint has an error in the case assignment document 
        /// create_campaign C1 P1 5 20 100 Campaign created; name C1, product P1, duration 10, limit 20, target sales count 100
        /// duration is 5 in the case assignment document but 10 in the output
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>

        [HttpPost("createCampaign")]
        public async Task<ActionResult<ApiResponse<CreateCampaignResponseDTO>>> CreateProductAsync([FromBody] CreateCampaignRequestDTO campaign)
        {
            if (campaign == null)
                return BadRequest("Campaign is required.");

            var createdCampaign = await _campaignService.CreateCampaignAsync(campaign);

            return Ok(new ApiResponse<CreateCampaignResponseDTO>(new CreateCampaignResponseDTO
            {
                Name = createdCampaign.Name,
                Duration = createdCampaign.Duration,
                Limit = createdCampaign.PriceManipulationLimit,
                Product = createdCampaign.ProductCode,
                TargetSalesCount = createdCampaign.TargetSalesCount
            }, message: "Campaign created"));
        }
    }
}
