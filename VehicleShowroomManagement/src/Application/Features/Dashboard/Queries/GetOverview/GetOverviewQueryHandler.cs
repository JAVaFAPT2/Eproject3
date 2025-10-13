namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetOverview
{
    public class GetOverviewQueryHandler(
        IRepository<Order> orderRepository,
        IRepository<ServiceOrder> serviceOrderRepository,
        IRepository<PurchaseOrder> purchaseOrderRepository,
        IRepository<User> userRepository,
        IRepository<VehicleModel> modelRepository,
        IRepository<Vehicle> vehicleRepository) : IRequestHandler<GetOverviewQuery, OverviewDto>
    {
        public async Task<OverviewDto> Handle(GetOverviewQuery request, CancellationToken cancellationToken)
        {
            var completedOrders = await orderRepository.FindAsync(o => o.Status == OrderStatus.Completed, cancellationToken);
            var completedOrdersList = completedOrders.ToList();
            var completedCount = completedOrdersList.Count;

            var totalSale = completedOrdersList.Sum(o => o.SalePrice);
            var serviceOrders = await serviceOrderRepository.GetAllAsync(cancellationToken);
            var totalService = serviceOrders.Where(s => s.Status == ServiceOrderStatus.Completed).Sum(s => s.Cost);

            var pos = await purchaseOrderRepository.GetAllAsync(cancellationToken);
            var purchaseCost = pos.Sum(p => p.TotalAmount);

            var employees = await userRepository.FindAsync(u => u.HireDate != null && u.DeletedAt == null, cancellationToken);
            var employeeCount = employees.Count();

            var customersPurchased = completedOrdersList.Select(o => o.CustomerId).Distinct().Count();

            var level2Models = await modelRepository.FindAsync(m => m.Level == 2, cancellationToken);
            var level2Count = level2Models.Count();

            var vehicles = await vehicleRepository.GetAllAsync(cancellationToken);
            var vehicleCount = vehicles.Count();

            return new OverviewDto
            {
                Profit = (totalSale + totalService) - purchaseCost,
                Employees = employeeCount,
                CustomersPurchased = customersPurchased,
                CompletedOrders = completedCount,
                Level2Models = level2Count,
                Vehicles = vehicleCount
            };
        }
    }
}


