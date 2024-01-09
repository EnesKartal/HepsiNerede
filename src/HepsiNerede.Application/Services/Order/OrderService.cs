using HepsiNerede.Application.Services.BaseService;
using HepsiNerede.Application.Services.TimeSimulation;
using HepsiNerede.Domain.Aggregates.OrderAggregate;
using System;
using System.Threading.Tasks;

namespace HepsiNerede.Application.Services.Order
{
    /// <summary>
    /// Service for handling order-related operations.
    /// </summary>
    public interface IOrderService : IBaseService
    {
        /// <summary>
        /// Creates an order asynchronously.
        /// </summary>
        /// <param name="createOrderRequestDTO">The DTO containing order creation information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created order.</returns>
        Task<Domain.Entities.Order> CreateOrderAsync(CreateOrderDTO createOrderRequestDTO);

        /// <summary>
        /// Retrieves orders for a campaign product within a specified time range.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="campaignStartTime">The start time of the campaign.</param>
        /// <param name="campaignEndTime">The end time of the campaign.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an array of orders for the specified campaign product.</returns>
        Task<Domain.Entities.Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignStartTime, DateTime campaignEndTime);
    }

    /// <summary>
    /// Implementation of <see cref="IOrderService"/> for handling order-related operations.
    /// </summary>
    public class OrderService : BaseService<IOrderRepository>, IOrderService
    {
        private readonly ITimeSimulationService _timeSimulationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="repository">The repository for order-related data.</param>
        /// <param name="timeSimulationService">The service for time simulation.</param>
        public OrderService(IOrderRepository repository, ITimeSimulationService timeSimulationService) : base(repository)
        {
            _timeSimulationService = timeSimulationService ?? throw new ArgumentNullException(nameof(timeSimulationService));
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<Domain.Entities.Order[]> GetOrdersForCampaignProductAsync(string productCode, DateTime campaignStartTime, DateTime campaignEndTime)
        {
            return await _repository.GetOrdersForCampaignProductAsync(productCode, campaignStartTime, campaignEndTime);
        }
    }
}
