using HepsiNerede.Application.DTOs.Order;
using HepsiNerede.Application.Services.Order;
using HepsiNerede.WebApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace HepsiNerede.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderCreationService _orderCreationService;

        public OrderController(IOrderCreationService orderCreationService)
        {
            _orderCreationService = orderCreationService;
        }

        [HttpPost("createOrder")]
        public async Task<ActionResult<ApiResponse<CreateOrderResponseDTO>>> CreateProductAsync([FromBody] CreateOrderRequestDTO request)
        {
            if (request == null)
                return BadRequest("Order is required.");

            if (string.IsNullOrEmpty(request.ProductCode))
                return BadRequest("Product code is required.");

            if (request.Quantity <= 0)
                return BadRequest("Quantity must be greater than 0.");

            var product = await _orderCreationService.CreateOrderAsync(request);
            return Ok(new ApiResponse<CreateOrderResponseDTO>(product, message: "Order created"));
        }
    }
}
