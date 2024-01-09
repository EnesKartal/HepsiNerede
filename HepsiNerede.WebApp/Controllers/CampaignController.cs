using HepsiNerede.Models.DTO.Campaign.CreateCampaign;
using HepsiNerede.Models.DTO.Product.GetProduct;
using HepsiNerede.Services;
using HepsiNerede.WebApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace HepsiNerede.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet("getCampaignByName")]
        public async Task<ActionResult<ApiResponse<GetCampaignResponseDTO>>> GetCampaignByNameAsync([FromQuery] string? name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Campaign name is required.");

            var campaign = await _campaignService.GetCampaignByNameAsync(name);

            if (campaign == null)
                return NotFound($"Campaign with name '{name}' not found.");

            return Ok(new ApiResponse<GetCampaignResponseDTO>(campaign, message: $"Campaign {name} info"));
        }

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
