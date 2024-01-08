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
        public ActionResult<ApiResponse<GetCampaignResponseDTO>> GetCampaignByName([FromQuery] string? name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Campaign name is required.");

            var campaign = _campaignService.GetCampaignByName(name);

            if (campaign == null)
                return NotFound($"Campaign with name '{name}' not found.");

            return Ok(new ApiResponse<GetCampaignResponseDTO>(new GetCampaignResponseDTO
            {
                AverageItemPrice = -1,
                Status = false,
                TargetSales = -1,
                TotalSales = -1,
                Turnover = -1
            }, message: $"Campaign {name} info"));
        }

        [HttpPost("createCampaign")]
        public ActionResult<ApiResponse<CreateCampaignResponseDTO>> CreateProduct([FromBody] CreateCampaignRequestDTO campaign)
        {
            if (campaign == null)
                return BadRequest("Campaign is required.");

            var createdCampaign = _campaignService.CreateCampaign(campaign);

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
