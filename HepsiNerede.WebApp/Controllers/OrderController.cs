using HepsiNerede.Models.DTO.Order;
using HepsiNerede.Services;
using HepsiNerede.WebApp.Common;
using Microsoft.AspNetCore.Mvc;

namespace HepsiNerede.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("createOrder")]
        public ActionResult<ApiResponse<CreateOrderResponseDTO>> CreateProduct([FromBody] CreateOrderRequestDTO order)
        {
            if (order == null)
                return BadRequest("Order is required.");

            var createdOrder = _orderService.CreateOrder(order);

            return Ok(new ApiResponse<CreateOrderResponseDTO>(new CreateOrderResponseDTO { Product = createdOrder.ProductCode, Quantity = createdOrder.Quantity }, message: "Order created"));
        }
    }
}
