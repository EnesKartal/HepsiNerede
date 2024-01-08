public interface IOrderService
{
    void AddOrder(string productCode, decimal quantity);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public void AddOrder(string productCode, decimal quantity)
    {
        var newOrder = new Order
        {
            ProductCode = productCode,
            Quantity = quantity
        };

        _orderRepository.AddOrder(newOrder);
    }
}