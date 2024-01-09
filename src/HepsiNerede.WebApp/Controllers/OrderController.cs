using HepsiNerede.Application.DTOs.Order;
using HepsiNerede.Application.Services.Order;
using HepsiNerede.WebApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace HepsiNerede.WebApp.Controllers
{
    /// <summary>
    /// API controller for managing orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderCreationService _orderCreationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderCreationService">The order creation service.</param>
        public OrderController(IOrderCreationService orderCreationService)
        {
            _orderCreationService = orderCreationService;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="request">The order information for creation.</param>
        /// <returns>Returns the created order details wrapped in an API response.</returns>
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
