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
        private readonly IProductService _productService;

        public OrderController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        [HttpPost("createOrder")]
        public async Task<ActionResult<ApiResponse<CreateOrderResponseDTO>>> CreateProductAsync([FromBody] CreateOrderRequestDTO order)
        {
            if (order == null)
                return BadRequest("Order is required.");

            if (string.IsNullOrEmpty(order.ProductCode))
                return BadRequest("Product code is required.");

            if (order.Quantity <= 0)
                return BadRequest("Quantity must be greater than 0.");

            var product = await _productService.GetProductByCodeAsync(order.ProductCode);
            if (product == null)
                return BadRequest("Product not found.");

            if (product.Stock < order.Quantity)
                return BadRequest("Product stock is not enough.");

            decimal totalPrice = product.Price * order.Quantity;

            order.TotalPrice = totalPrice;

            //TODO: Transaction
            await _productService.DecreaseProductStockAsync(order.ProductCode, order.Quantity);

            var createdOrder = await _orderService.CreateOrderAsync(order);

            return Ok(new ApiResponse<CreateOrderResponseDTO>(new CreateOrderResponseDTO { Product = createdOrder.ProductCode, Quantity = createdOrder.Quantity }, message: "Order created"));
        }
    }
}
