using HepsiNerede.Data.Entities;
using HepsiNerede.Data.Repositories;
using HepsiNerede.Models.DTO.Order;

namespace HepsiNerede.Services
{
    public interface IOrderService
    {
        Order CreateOrder(CreateOrderRequestDTO createOrderRequestDTO);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order CreateOrder(CreateOrderRequestDTO createOrderRequestDTO)
        {
            var newOrder = new Order
            {
                ProductCode = createOrderRequestDTO.ProductCode,
                Quantity = createOrderRequestDTO.Quantity
            };

            return _orderRepository.CreateOrder(newOrder);
        }
    }
}
