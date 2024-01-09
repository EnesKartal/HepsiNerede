using HepsiNerede.Data.Entities;

namespace HepsiNerede.Data.Repositories
{
    public interface IOrderRepository
    {
        Order CreateOrder(Order product);
        Order[] GetOrdersForCampaignProduct(string productCode, DateTime campaignEndTime);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly HepsiNeredeDBContext _dbContext;

        public OrderRepository(HepsiNeredeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Order CreateOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
            return order;
        }

        public Order[] GetOrdersForCampaignProduct(string productCode, DateTime campaignEndTime)
        {
            return _dbContext.Orders.Where(x => x.ProductCode == productCode && x.CreatedAt <= campaignEndTime).ToArray();
        }
    }
}
