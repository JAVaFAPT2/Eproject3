namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetOverview
{
    public class GetOverviewQueryHandler(
        IRepository<Order> orderRepository,
        IRepository<ServiceOrder> serviceOrderRepository,
        IRepository<PurchaseOrder> purchaseOrderRepository,
        IRepository<User> userRepository,
        IRepository<VehicleModel> modelRepository,
        IRepository<Vehicle> vehicleRepository,
        ILogger<GetOverviewQueryHandler> logger) : IRequestHandler<GetOverviewQuery, OverviewDto>
    {
        public async Task<OverviewDto> Handle(GetOverviewQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting dashboard overview calculation");

            try
            {
                var completedOrders = await orderRepository.FindAsync(o => o.Status == OrderStatus.Completed, cancellationToken);
                var completedOrdersList = completedOrders.ToList();
                var completedCount = completedOrdersList.Count;

                logger.LogDebug("Found {CompletedOrdersCount} completed orders", completedCount);

                var totalSale = completedOrdersList.Sum(o => o.SalePrice);
                var serviceOrders = await serviceOrderRepository.GetAllAsync(cancellationToken);
                var totalService = serviceOrders.Where(s => s.Status == ServiceOrderStatus.Completed).Sum(s => s.Cost);

                logger.LogDebug("Total sales: {TotalSale}, Total service revenue: {TotalService}", totalSale, totalService);

                var pos = await purchaseOrderRepository.GetAllAsync(cancellationToken);
                var purchaseCost = pos.Sum(p => p.TotalAmount);

                var employees = await userRepository.FindAsync(u => u.HireDate != null && u.DeletedAt == null, cancellationToken);
                var employeeCount = employees.Count();

                var customersPurchased = completedOrdersList.Select(o => o.CustomerId).Distinct().Count();

                var level2Models = await modelRepository.FindAsync(m => m.Level == 2, cancellationToken);
                var level2Count = level2Models.Count();

                var vehicles = await vehicleRepository.GetAllAsync(cancellationToken);
                var vehicleCount = vehicles.Count();

                var profit = (totalSale + totalService) - purchaseCost;

                logger.LogInformation("Dashboard overview calculated successfully - Profit: {Profit}, Employees: {Employees}, Customers: {Customers}, Orders: {Orders}, Models: {Models}, Vehicles: {Vehicles}", 
                    profit, employeeCount, customersPurchased, completedCount, level2Count, vehicleCount);

                return new OverviewDto
                {
                    Profit = profit,
                    Employees = employeeCount,
                    CustomersPurchased = customersPurchased,
                    CompletedOrders = completedCount,
                    Level2Models = level2Count,
                    Vehicles = vehicleCount
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error calculating dashboard overview");
                throw;
            }
        }
    }
}


