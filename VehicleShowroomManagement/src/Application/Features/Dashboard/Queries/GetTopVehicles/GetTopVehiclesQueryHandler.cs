
namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetTopVehicles
{
    /// <summary>
    /// Handler for get top vehicles query (updated for new Order/Vehicle schema)
    /// </summary>
    public class GetTopVehiclesQueryHandler(
        IRepository<Order> orderRepository,
        IRepository<Vehicle> vehicleRepository,
        IRepository<VehicleModel> modelRepository) : IRequestHandler<GetTopVehiclesQuery, List<TopVehicleDto>>
    {
        public async Task<List<TopVehicleDto>> Handle(GetTopVehiclesQuery request, CancellationToken cancellationToken)
        {
            var allOrders = await orderRepository.GetAllAsync(cancellationToken);

            // Default to current month if no date range provided
            DateTime? effectiveFrom = request.FromDate;
            DateTime? effectiveTo = request.ToDate;
            if (effectiveFrom == null && effectiveTo == null)
            {
                var now = DateTime.UtcNow;
                var firstOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                var firstOfNextMonth = firstOfMonth.AddMonths(1);
                effectiveFrom = firstOfMonth;
                effectiveTo = firstOfNextMonth;
            }

            // Filter by completed orders within date range
            var filteredOrders = allOrders
                .Where(o => o.Status == OrderStatus.Completed &&
                           (effectiveFrom == null || o.OrderDate >= effectiveFrom) &&
                           (effectiveTo == null || o.OrderDate < effectiveTo))
                .ToList();

            // Group by level-2 model (ModelNumber is the level-2 identifier)
            var topModels = filteredOrders
                .GroupBy(o => o.ModelNumber)
                .Select(g => new
                {
                    ModelNumber = g.Key,
                    Count = g.Count(),
                    TotalRevenue = g.Sum(o => o.SalePrice),
                    AveragePrice = g.Average(o => o.SalePrice),
                    LastSaleDate = g.Max(o => o.OrderDate)
                })
                .OrderByDescending(x => x.Count)
                .Take(request.Top)
                .ToList();

            var result = new List<TopVehicleDto>();
            
            foreach (var item in topModels)
            {
                var models = await modelRepository.FindAsync(m => m.ModelNumber == item.ModelNumber, cancellationToken);
                var model = models.FirstOrDefault();

                // Count available stock for this level-2 model
                var allVehicles = await vehicleRepository.FindAsync(v => v.ModelNumber == item.ModelNumber, cancellationToken);
                var availableStock = allVehicles.Count(v => v.Status == VehicleStatus.Available);

                result.Add(new TopVehicleDto
                {
                    VehicleId = "", // Not applicable for model-level stats
                    ModelNumber = item.ModelNumber,
                    Brand = "",
                    Model = model?.Name ?? item.ModelNumber,
                    SalesCount = item.Count,
                    TotalRevenue = item.TotalRevenue,
                    AveragePrice = item.AveragePrice,
                    LastSaleDate = item.LastSaleDate,
                    AvailableStock = availableStock
                });
            }

            return result;
        }
    }
}
