using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Domain.Aggregates.OrderAggregate;
using HepsiNerede.Domain.Entities;
using HepsiNerede.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HepsiNerede.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for accessing order-related data.
    /// </summary>
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to be used by the repository.</param>
        public OrderRepository(HepsiNeredeDbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Creates a new order asynchronously.
        /// </summary>
        /// <param name="order">The order to be created.</param>
        /// <returns>The created order.</returns>
        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        /// <summary>
        /// Gets orders for a campaign product within the specified time range asynchronously.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="campaignStartTime">The start time of the campaign.</param>
        /// <param name="campaignEndTime">The end time of the campaign.</param>
        /// <returns>An array of orders for the campaign product within the specified time range.</returns>
        public async Task<Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignStartTime, DateTime campaignEndTime)
        {
            return await _dbContext.Orders
                .Where(x => x.ProductCode == productCode && x.CreatedAt >= campaignStartTime && x.CreatedAt <= campaignEndTime)
                .ToArrayAsync();
        }
    }
}
