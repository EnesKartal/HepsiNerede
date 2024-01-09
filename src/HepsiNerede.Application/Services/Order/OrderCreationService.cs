using HepsiNerede.Application.DTOs.Order;
using HepsiNerede.Application.Services.Product;
using System;
using System.Threading.Tasks;

namespace HepsiNerede.Application.Services.Order
{
    /// <summary>
    /// Service for handling the creation of orders.
    /// </summary>
    public interface IOrderCreationService
    {
        /// <summary>
        /// Creates an order asynchronously.
        /// </summary>
        /// <param name="createOrderRequestDTO">The DTO containing order creation information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created order.</returns>
        Task<CreateOrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO createOrderRequestDTO);
    }

    /// <summary>
    /// Implementation of <see cref="IOrderCreationService"/> for handling the creation of orders.
    /// </summary>
    public class OrderCreationService : IOrderCreationService
    {
        private readonly IProductCampaignService _productCampaignService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderCreationService"/> class.
        /// </summary>
        /// <param name="productCampaignService">The service for product campaigns.</param>
        /// <param name="orderService">The order service.</param>
        /// <param name="productService">The product service.</param>
        public OrderCreationService(
            IProductCampaignService productCampaignService,
            IOrderService orderService,
            IProductService productService)
        {
            _productCampaignService = productCampaignService ?? throw new ArgumentNullException(nameof(productCampaignService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        /// <inheritdoc/>
        public async Task<CreateOrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO request)
        {
            var product = await _productCampaignService.GetProductByCodeAsync(request.ProductCode);
            if (product == null)
                throw new Exception("Product not found.");

            decimal totalPrice = product.Price * request.Quantity;
            try
            {
                await _orderService.BeginTransactionAsync();
                await _productService.DecreaseProductStockAsync(request.ProductCode, request.Quantity);

                var createdOrder = await _orderService.CreateOrderAsync(new CreateOrderDTO
                {
                    ProductCode = request.ProductCode,
                    Quantity = request.Quantity,
                    TotalPrice = totalPrice
                });

                await _orderService.CommitTransactionAsync();
                return new CreateOrderResponseDTO { Product = createdOrder.ProductCode, Quantity = createdOrder.Quantity };
            }
            catch (Exception)
            {
                await _orderService.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
