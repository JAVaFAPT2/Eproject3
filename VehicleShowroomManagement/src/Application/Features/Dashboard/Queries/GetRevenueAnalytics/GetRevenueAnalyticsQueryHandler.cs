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

            // Determine analysis window: default to last 6 full months including current month
            var now = DateTime.UtcNow;
            var startOfCurrentMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var analysisStart = request.FromDate ?? startOfCurrentMonth.AddMonths(-5);
            var analysisEndExclusive = request.ToDate ?? startOfCurrentMonth.AddMonths(1);

            // Aggregate revenue per month over the 6-month window
            var revenueSeries = new List<RevenueDataPoint>();
            var cursor = new DateTime(analysisStart.Year, analysisStart.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            while (cursor < analysisEndExclusive)
            {
                var next = cursor.AddMonths(1);
                var monthOrders = completedOrders.Where(o => o.OrderDate >= cursor && o.OrderDate < next).ToList();
                var monthRevenue = monthOrders.Sum(o => o.SalePrice);
                revenueSeries.Add(new RevenueDataPoint
                {
                    Label = cursor.ToString("yyyy-MM"),
                    Value = monthRevenue,
                    Date = cursor
                });
                cursor = next;
            }

            // Totals for current month and previous month
            var currentMonthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var previousMonthStart = currentMonthStart.AddMonths(-1);
            var currentMonthRevenue = completedOrders.Where(o => o.OrderDate >= currentMonthStart && o.OrderDate < currentMonthStart.AddMonths(1)).Sum(o => o.SalePrice);
            var previousMonthRevenue = completedOrders.Where(o => o.OrderDate >= previousMonthStart && o.OrderDate < currentMonthStart).Sum(o => o.SalePrice);
            var growthPercentage = previousMonthRevenue > 0 ? ((currentMonthRevenue - previousMonthRevenue) / previousMonthRevenue) * 100 : 0;

            // Overall metrics for the selected window
            var windowOrders = completedOrders.Where(o => o.OrderDate >= analysisStart && o.OrderDate < analysisEndExclusive).ToList();
            var totalRevenue = windowOrders.Sum(o => o.SalePrice);

            return new RevenueAnalyticsResult
            {
                TotalRevenue = totalRevenue,
                PreviousPeriodRevenue = previousMonthRevenue,
                GrowthPercentage = growthPercentage,
                RevenueData = revenueSeries,
                RevenueByCategory = new List<RevenueByCategory>(),
                AverageOrderValue = windowOrders.Any() ? windowOrders.Average(o => o.SalePrice) : 0,
                TotalOrders = windowOrders.Count
            };
        }
    }
}
