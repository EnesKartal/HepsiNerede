using HepsiNerede.Domain.Entities;
using HepsiNerede.Infrastructure.DbContexts;
using HepsiNerede.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HepsiNerede.Domain.Aggregates.OrderAggregate
{

    public class OrderRepository : BaseRepository, IOrderRepository
    {

        public OrderRepository(HepsiNeredeDBContext dbContext) : base(dbContext) { }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignStartTime, DateTime campaignEndTime)
        {
            return await _dbContext.Orders.Where(
                x => x.ProductCode == productCode
                && x.CreatedAt >= campaignStartTime
                && x.CreatedAt <= campaignEndTime)
                .ToArrayAsync();
        }
    }
}
