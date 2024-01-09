using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Order;

namespace HepsiNerede.Services
{
    public interface IOrderService
    {
        Order CreateOrder(CreateOrderRequestDTO createOrderRequestDTO);
        Order[] GetOrdersForCampaignProduct(string productCode, DateTime campaignEndTime);
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

        public Order CreateOrder(CreateOrderRequestDTO createOrderRequestDTO)
        {
            var newOrder = new Order
            {
                ProductCode = createOrderRequestDTO.ProductCode,
                Quantity = createOrderRequestDTO.Quantity,
                CreatedAt = _timeSimulationService.GetCurrentTime(),
                TotalPrice = createOrderRequestDTO.TotalPrice
            };

            return _orderRepository.CreateOrder(newOrder);
        }

        public Order[] GetOrdersForCampaignProduct(string productCode, DateTime campaignEndTime)
        {
            return _orderRepository.GetOrdersForCampaignProduct(productCode, campaignEndTime);
        }
    }
}
