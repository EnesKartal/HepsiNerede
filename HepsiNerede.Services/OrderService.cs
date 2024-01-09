using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Order;

namespace HepsiNerede.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequestDTO createOrderRequestDTO);
        Task<Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignEndTime);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITimeSimulationService _timeSimulationService;

        public OrderService(IOrderRepository orderRepository, ITimeSimulationService timeSimulationService)
        {
            _orderRepository = orderRepository;
            _timeSimulationService = timeSimulationService;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequestDTO createOrderRequestDTO)
        {
            var newOrder = new Order
            {
                ProductCode = createOrderRequestDTO.ProductCode,
                Quantity = createOrderRequestDTO.Quantity,
                CreatedAt = _timeSimulationService.GetCurrentTime(),
                TotalPrice = createOrderRequestDTO.TotalPrice
            };

            return await _orderRepository.CreateOrderAsync(newOrder);
        }

        public async Task<Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignEndTime)
        {
            return await _orderRepository.GetOrdersForCampaignProductAsync(productCode, campaignEndTime);
        }
    }
}
