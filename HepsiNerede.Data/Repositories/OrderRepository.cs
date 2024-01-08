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

        public void AddOrder(Order Order)
        {
            _dbContext.Orders.Add(Order);
            _dbContext.SaveChanges();
        }
    }
}
