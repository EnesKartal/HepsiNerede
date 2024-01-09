using HepsiNerede.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order product);
        Task<Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignEndTime);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly HepsiNeredeDBContext _dbContext;

        public OrderRepository(HepsiNeredeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignEndTime)
        {
            return await _dbContext.Orders.Where(x => x.ProductCode == productCode && x.CreatedAt <= campaignEndTime).ToArrayAsync();
        }
    }
}
