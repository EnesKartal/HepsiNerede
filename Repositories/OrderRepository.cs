public interface IOrderRepository
{
    void AddOrder(Order product);
}

public class OrderRepository : IOrderRepository
{
    private readonly List<Order> _orders;

    public OrderRepository()
    {
        _orders = new List<Order>();
    }

    public void AddOrder(Order Order)
    {
        _orders.Add(Order);
    }
}
