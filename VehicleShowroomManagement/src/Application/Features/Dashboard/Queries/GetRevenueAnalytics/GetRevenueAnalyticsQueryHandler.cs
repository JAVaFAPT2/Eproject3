namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRevenueAnalytics
{
    /// <summary>
    /// Handler for get revenue analytics query (updated for new Order schema)
    /// </summary>
    public class GetRevenueAnalyticsQueryHandler(IRepository<Order> orderRepository) : IRequestHandler<GetRevenueAnalyticsQuery, RevenueAnalyticsResult>
    {
        public async Task<RevenueAnalyticsResult> Handle(GetRevenueAnalyticsQuery request, CancellationToken cancellationToken)
        {
            var allOrders = await orderRepository.GetAllAsync(cancellationToken);
            
            // Filter completed orders
            var completedOrders = allOrders.Where(o => o.Status == OrderStatus.Completed).ToList();

            // Calculate revenue for current period
            var currentPeriodStart = request.FromDate ?? DateTime.UtcNow.AddMonths(-1);
            var currentPeriodOrders = completedOrders.Where(o => o.OrderDate >= currentPeriodStart).ToList();
            var totalRevenue = currentPeriodOrders.Sum(o => o.SalePrice);

            // Calculate previous period for comparison
            var previousPeriodStart = currentPeriodStart.AddMonths(-1);
            var previousPeriodOrders = completedOrders
                .Where(o => o.OrderDate >= previousPeriodStart && o.OrderDate < currentPeriodStart)
                .ToList();
            var previousRevenue = previousPeriodOrders.Sum(o => o.SalePrice);

            var growthPercentage = previousRevenue > 0 
                ? ((totalRevenue - previousRevenue) / previousRevenue) * 100 
                : 0;

            return new RevenueAnalyticsResult
            {
                TotalRevenue = totalRevenue,
                PreviousPeriodRevenue = previousRevenue,
                GrowthPercentage = growthPercentage,
                RevenueData = new List<RevenueDataPoint>(),
                RevenueByCategory = new List<RevenueByCategory>(),
                AverageOrderValue = currentPeriodOrders.Any() ? currentPeriodOrders.Average(o => o.SalePrice) : 0,
                TotalOrders = currentPeriodOrders.Count
            };
        }
    }
}
