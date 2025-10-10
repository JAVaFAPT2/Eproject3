using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRecentOrders
{
    /// <summary>
    /// Handler for get recent orders query (updated for new Order schema)
    /// </summary>
    public class GetRecentOrdersQueryHandler : IRequestHandler<GetRecentOrdersQuery, List<RecentOrderDto>>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<VehicleModel> _modelRepository;

        public GetRecentOrdersQueryHandler(
            IRepository<Order> orderRepository,
            IRepository<User> userRepository,
            IRepository<VehicleModel> modelRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _modelRepository = modelRepository;
        }

        public async Task<List<RecentOrderDto>> Handle(GetRecentOrdersQuery request, CancellationToken cancellationToken)
        {
            var allOrders = await _orderRepository.GetAllAsync();
            
            var recentOrders = allOrders
                .OrderByDescending(o => o.OrderDate)
                .Take(request.Limit)
                .ToList();

            var result = new List<RecentOrderDto>();

            foreach (var order in recentOrders)
            {
                // Get customer
                var customer = await _userRepository.GetByIdAsync(order.CustomerId);
                
                // Get dealer
                var dealer = await _userRepository.GetByIdAsync(order.DealerId);
                
                // Get model
                var models = await _modelRepository.FindAsync(m => m.ModelNumber == order.ModelNumber);
                var model = models.FirstOrDefault();

                result.Add(new RecentOrderDto
                {
                    OrderId = order.Id,
                    OrderNumber = order.Id, // Using ID as order number
                    CustomerName = customer?.Name ?? "Unknown",
                    VehicleModel = model?.Name ?? order.ModelNumber,
                    TotalAmount = order.SalePrice,
                    Status = order.Status.ToString(),
                    OrderDate = order.OrderDate,
                    SalesPersonName = dealer?.Name ?? "Unknown"
                });
            }

            return result;
        }
    }
}
