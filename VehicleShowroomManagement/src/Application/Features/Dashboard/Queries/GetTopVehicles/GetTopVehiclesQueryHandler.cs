using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetTopVehicles
{
    /// <summary>
    /// Handler for get top vehicles query (updated for new Order/Vehicle schema)
    /// </summary>
    public class GetTopVehiclesQueryHandler : IRequestHandler<GetTopVehiclesQuery, List<TopVehicleDto>>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<VehicleModel> _modelRepository;

        public GetTopVehiclesQueryHandler(
            IRepository<Order> orderRepository,
            IRepository<Vehicle> vehicleRepository,
            IRepository<VehicleModel> modelRepository)
        {
            _orderRepository = orderRepository;
            _vehicleRepository = vehicleRepository;
            _modelRepository = modelRepository;
        }

        public async Task<List<TopVehicleDto>> Handle(GetTopVehiclesQuery request, CancellationToken cancellationToken)
        {
            var allOrders = await _orderRepository.GetAllAsync();
            
            // Filter by date if provided
            var filteredOrders = allOrders
                .Where(o => o.Status == OrderStatus.Completed &&
                           (request.FromDate == null || o.OrderDate >= request.FromDate) &&
                           (request.ToDate == null || o.OrderDate <= request.ToDate))
                .ToList();

            // Group by model number and count
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
                var models = await _modelRepository.FindAsync(m => m.ModelNumber == item.ModelNumber);
                var model = models.FirstOrDefault();

                // Count available stock
                var allVehicles = await _vehicleRepository.FindAsync(v => v.ModelNumber == item.ModelNumber);
                var availableStock = allVehicles.Count(v => v.Status == VehicleStatus.InStock);

                result.Add(new TopVehicleDto
                {
                    VehicleId = "", // Not applicable for model-level stats
                    ModelNumber = item.ModelNumber,
                    Brand = model?.Brand ?? "Unknown",
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
