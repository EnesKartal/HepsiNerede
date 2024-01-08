using HepsiNerede.Data.Entities;

namespace HepsiNerede.Data.Repositories
{
    public interface IOrderRepository
    {
        void AddOrder(Order product);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly HepsiNeredeDBContext _dbContext;

        public OrderRepository(HepsiNeredeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddOrder(Order order)
        {
            order.CreatedAt = DateTime.Now;
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }
    }
}
