using HepsiNerede.Application.DTOs.Order;
using HepsiNerede.Application.Services.Product;

namespace HepsiNerede.Application.Services.Order
{
    public interface IOrderCreationService
    {
        Task<CreateOrderResponseDTO> CreateOrderAsync(CreateOrderRequestDTO createOrderRequestDTO);
    }

    public class OrderCreationService : IOrderCreationService
    {
        private readonly IProductCampaignService _productCampaignService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public OrderCreationService(IProductCampaignService productCampaignService, IOrderService orderService, IProductService productService)
        {
            _productCampaignService = productCampaignService;
            _orderService = orderService;
            _productService = productService;
        }

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
