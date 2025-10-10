using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetCustomerAnalytics
{
    /// <summary>
    /// Handler for get customer analytics query (updated for new User schema)
    /// </summary>
    public class GetCustomerAnalyticsQueryHandler : IRequestHandler<GetCustomerAnalyticsQuery, CustomerAnalyticsResult>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Order> _orderRepository;

        public GetCustomerAnalyticsQueryHandler(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IRepository<Order> orderRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _orderRepository = orderRepository;
        }

        public async Task<CustomerAnalyticsResult> Handle(GetCustomerAnalyticsQuery request, CancellationToken cancellationToken)
        {
            // Get customer role
            var roles = await _roleRepository.FindAsync(r => r.Name == "Customer");
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
            var allCustomers = await _userRepository.FindAsync(u => u.RoleId == customerRole.Id && u.DeletedAt == null);
            var totalCustomers = allCustomers.Count();

            // Calculate new customers (last 30 days)
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var newCustomers = allCustomers.Count(c => c.CreatedAt >= thirtyDaysAgo);

            // Calculate active customers (has orders)
            var allOrders = await _orderRepository.GetAllAsync();
            var activeCustomerIds = allOrders.Select(o => o.CustomerId).Distinct().ToList();
            var activeCustomers = allCustomers.Count(c => activeCustomerIds.Contains(c.Id));

            return new CustomerAnalyticsResult
            {
                TotalCustomers = totalCustomers,
                NewCustomers = newCustomers,
                ActiveCustomers = activeCustomers,
                CustomerGrowthPercentage = totalCustomers > 0 ? (newCustomers * 100m / totalCustomers) : 0,
                CustomerGrowthData = new List<CustomerDataPoint>(),
                TopCustomers = new List<TopCustomer>(),
                AverageCustomerValue = 0, // TODO: Calculate from completed orders
                RepeatCustomers = 0 // TODO: Calculate customers with >1 order
            };
        }
    }
}
