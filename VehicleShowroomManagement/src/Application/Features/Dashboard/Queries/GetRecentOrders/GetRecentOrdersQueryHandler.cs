
namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRecentOrders
{
    /// <summary>
    /// Handler for get recent orders query (updated for new Order schema)
    /// </summary>
    public class GetRecentOrdersQueryHandler(
        IRepository<Order> orderRepository,
        IRepository<User> userRepository,
        IRepository<VehicleModel> modelRepository) : IRequestHandler<GetRecentOrdersQuery, List<RecentOrderDto>>
    {
        public async Task<List<RecentOrderDto>> Handle(GetRecentOrdersQuery request, CancellationToken cancellationToken)
        {
            var allOrders = await orderRepository.GetAllAsync(cancellationToken);
            
            var recentOrders = allOrders
                .OrderByDescending(o => o.OrderDate)
                .Take(request.Limit)
                .ToList();

            var result = new List<RecentOrderDto>();

            foreach (var order in recentOrders)
            {
                // Get customer
                var customer = await userRepository.GetByIdAsync(order.CustomerId, cancellationToken);
                
                // Get dealer
                var dealer = order.DealerId != null ? await userRepository.GetByIdAsync(order.DealerId, cancellationToken) : null;
                
                // Get model
                var models = await modelRepository.FindAsync(m => m.ModelNumber == order.ModelNumber, cancellationToken);
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
