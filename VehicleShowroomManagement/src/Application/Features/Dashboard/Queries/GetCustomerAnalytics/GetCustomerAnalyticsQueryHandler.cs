namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetCustomerAnalytics
{
    /// <summary>
    /// Handler for get customer analytics query (updated for new User schema)
    /// </summary>
    public class GetCustomerAnalyticsQueryHandler(
        IRepository<User> userRepository,
        IRepository<Role> roleRepository,
        IRepository<Order> orderRepository) : IRequestHandler<GetCustomerAnalyticsQuery, CustomerAnalyticsResult>
    {
        public async Task<CustomerAnalyticsResult> Handle(GetCustomerAnalyticsQuery request, CancellationToken cancellationToken)
        {
            // Get customer role
            var roles = await roleRepository.FindAsync(r => r.Name == "Customer", cancellationToken);
            var customerRole = roles.FirstOrDefault();

            if (customerRole == null)
            {
                return new CustomerAnalyticsResult
                {
                    TotalCustomers = 0,
                    NewCustomers = 0,
                    ActiveCustomers = 0,
                    CustomerGrowthPercentage = 0,
                    CustomerGrowthData = new List<CustomerDataPoint>(),
                    TopCustomers = new List<TopCustomer>(),
                    AverageCustomerValue = 0,
                    RepeatCustomers = 0
                };
            }

            // Get all customers (users with Customer role)
            var allCustomers = (await userRepository.FindAsync(u => u.RoleId == customerRole.Id && u.DeletedAt == null, cancellationToken)).ToList();
            var totalCustomers = allCustomers.Count;

            // Build 6-month customer growth series including current month
            var now = DateTime.UtcNow;
            var startOfCurrentMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var seriesStart = startOfCurrentMonth.AddMonths(-5);
            var seriesEndExclusive = startOfCurrentMonth.AddMonths(1);

            var growthSeries = new List<CustomerDataPoint>();
            var cursor = new DateTime(seriesStart.Year, seriesStart.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            int cumulative = 0;
            while (cursor < seriesEndExclusive)
            {
                var next = cursor.AddMonths(1);
                var monthNew = allCustomers.Count(c => c.CreatedAt >= cursor && c.CreatedAt < next);
                cumulative += monthNew;
                growthSeries.Add(new CustomerDataPoint
                {
                    Label = cursor.ToString("yyyy-MM"),
                    NewCustomers = monthNew,
                    TotalCustomers = cumulative,
                    Date = cursor
                });
                cursor = next;
            }

            // New customers for current month
            var newCustomers = allCustomers.Count(c => c.CreatedAt >= startOfCurrentMonth);

            // Calculate active customers (has orders)
            var allOrders = await orderRepository.GetAllAsync(cancellationToken);
            var activeCustomerIds = allOrders.Select(o => o.CustomerId).Distinct().ToList();
            var activeCustomers = allCustomers.Count(c => activeCustomerIds.Contains(c.Id));

            // Compute average customer value and repeat customers from completed orders
            var completedOrders = allOrders.Where(o => o.Status == OrderStatus.Completed).ToList();
            var revenueByCustomer = completedOrders
                .GroupBy(o => o.CustomerId)
                .Select(g => new { CustomerId = g.Key, Total = g.Sum(o => o.SalePrice), Count = g.Count() })
                .ToList();
            var averageCustomerValue = revenueByCustomer.Any() ? revenueByCustomer.Average(x => x.Total) : 0m;
            var repeatCustomers = revenueByCustomer.Count(x => x.Count > 1);

            return new CustomerAnalyticsResult
            {
                TotalCustomers = totalCustomers,
                NewCustomers = newCustomers,
                ActiveCustomers = activeCustomers,
                CustomerGrowthPercentage = totalCustomers > 0 ? (newCustomers * 100m / totalCustomers) : 0,
                CustomerGrowthData = growthSeries,
                TopCustomers = new List<TopCustomer>(),
                AverageCustomerValue = averageCustomerValue,
                RepeatCustomers = repeatCustomers
            };
        }
    }
}
