using HepsiNerede.Domain.Aggregates.Base;
using HepsiNerede.Domain.Entities;

namespace HepsiNerede.Domain.Aggregates.OrderAggregate
{
    public interface IOrderRepository : IBaseRepository
    {
        Task<Order> CreateOrderAsync(Order product);
        Task<Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignStartTime, DateTime campaignEndTime);
    }
}
