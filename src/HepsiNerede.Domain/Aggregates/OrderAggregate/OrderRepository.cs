using System;
using System.Threading.Tasks;
using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Domain.Entities;

namespace HepsiNerede.Domain.Aggregates.OrderAggregate
{
    /// <summary>
    /// Interface for order repository operations.
    /// </summary>
    public interface IOrderRepository : IBaseRepository
    {
        /// <summary>
        /// Creates a new order asynchronously.
        /// </summary>
        /// <param name="order">The order to be created.</param>
        /// <returns>A task representing the asynchronous operation, containing the created order.</returns>
        Task<Order> CreateOrderAsync(Order order);

        /// <summary>
        /// Gets orders for a campaign product within a specified time range asynchronously.
        /// </summary>
        /// <param name="productCode">The product code for which to retrieve orders.</param>
        /// <param name="campaignStartTime">The start time of the campaign.</param>
        /// <param name="campaignEndTime">The end time of the campaign.</param>
        /// <returns>A task representing the asynchronous operation, containing an array of orders.</returns>
        Task<Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignStartTime, DateTime campaignEndTime);
    }
}
