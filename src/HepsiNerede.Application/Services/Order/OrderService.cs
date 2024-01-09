using HepsiNerede.Application.Services.BaseService;
using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.Domain.Aggregates.OrderAggregate;

namespace HepsiNerede.Application.Services.Order
{
    public interface IOrderService : IBaseService
    {
        Task<Domain.Entities.Order> CreateOrderAsync(CreateOrderDTO createOrderRequestDTO);
        Task<Domain.Entities.Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignStartTime, DateTime campaignEndTime);
    }

    public class OrderService : BaseService<IOrderRepository>, IOrderService
    {
        private readonly ITimeSimulationService _timeSimulationService;

        public OrderService(IOrderRepository repository, ITimeSimulationService timeSimulationService) : base(repository)
        {
            _timeSimulationService = timeSimulationService;
        }

        public async Task<Domain.Entities.Order> CreateOrderAsync(CreateOrderDTO createOrderRequestDTO)
        {
            var newOrder = new Domain.Entities.Order
            {
                ProductCode = createOrderRequestDTO.ProductCode,
                Quantity = createOrderRequestDTO.Quantity,
                CreatedAt = _timeSimulationService.GetCurrentTime(),
                TotalPrice = createOrderRequestDTO.TotalPrice
            };

            return await _repository.CreateOrderAsync(newOrder);
        }

        public async Task<Domain.Entities.Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignStartTime, DateTime campaignEndTime)
        {
            return await _repository.GetOrdersForCampaignProductAsync(productCode, campaignStartTime, campaignEndTime);
        }
    }
}
