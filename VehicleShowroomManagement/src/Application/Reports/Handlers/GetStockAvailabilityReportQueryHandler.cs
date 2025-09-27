using MediatR;
using VehicleShowroomManagement.Application.Reports.DTOs;
using VehicleShowroomManagement.Application.Reports.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.Reports.Handlers
{
    /// <summary>
    /// Handler for getting stock availability report
    /// </summary>
    public class GetStockAvailabilityReportQueryHandler : IRequestHandler<GetStockAvailabilityReportQuery, StockAvailabilityReportDto>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public GetStockAvailabilityReportQueryHandler(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<StockAvailabilityReportDto> Handle(GetStockAvailabilityReportQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            var asOfDate = request.AsOfDate ?? DateTime.UtcNow;

            // Apply filters (simplified for new schema)
            if (!string.IsNullOrEmpty(request.Status))
            {
                vehicles = vehicles.Where(v => v.Status == request.Status);
            }

            // Brand and Model filtering not directly available in new schema

            // Filter by date if specified
            if (request.AsOfDate.HasValue)
            {
                vehicles = vehicles.Where(v => v.CreatedAt <= request.AsOfDate.Value);
            }

            var vehicleList = vehicles.ToList();

            var report = new StockAvailabilityReportDto
            {
                GeneratedAt = DateTime.UtcNow,
                AsOfDate = asOfDate,
                TotalVehicles = vehicleList.Count,
                AvailableVehicles = vehicleList.Count(v => v.Status == "Available"),
                SoldVehicles = vehicleList.Count(v => v.Status == "Sold"),
                ReservedVehicles = vehicleList.Count(v => v.Status == "Reserved"),
                InServiceVehicles = vehicleList.Count(v => v.Status == "InService"),
                TotalValue = vehicleList.Sum(v => v.PurchasePrice),
                AvailableValue = vehicleList.Where(v => v.Status == "Available").Sum(v => v.PurchasePrice)
            };

            // Generate brand-wise stock (simplified for new schema)
            report.BrandStocks = new List<BrandStockDto>
            {
                new BrandStockDto
                {
                    Brand = "All",
                    TotalCount = vehicleList.Count,
                    AvailableCount = vehicleList.Count(v => v.Status == "Available"),
                    SoldCount = vehicleList.Count(v => v.Status == "Sold"),
                    ReservedCount = vehicleList.Count(v => v.Status == "Reserved"),
                    InServiceCount = vehicleList.Count(v => v.Status == "InService"),
                    TotalValue = vehicleList.Sum(v => v.PurchasePrice),
                    AvailableValue = vehicleList.Where(v => v.Status == "Available").Sum(v => v.PurchasePrice)
                }
            };

            // Generate model-wise stock (simplified for new schema)
            report.ModelStocks = vehicleList
                .GroupBy(v => v.ModelNumber)
                .Select(g => new ModelStockDto
                {
                    Brand = "Unknown",
                    Model = g.Key,
                    TotalCount = g.Count(),
                    AvailableCount = g.Count(v => v.Status == "Available"),
                    SoldCount = g.Count(v => v.Status == "Sold"),
                    ReservedCount = g.Count(v => v.Status == "Reserved"),
                    InServiceCount = g.Count(v => v.Status == "InService"),
                    TotalValue = g.Sum(v => v.PurchasePrice),
                    AvailableValue = g.Where(v => v.Status == "Available").Sum(v => v.PurchasePrice),
                    AveragePrice = g.Average(v => v.PurchasePrice)
                })
                .OrderByDescending(m => m.TotalCount)
                .ToList();

            // Generate status-wise stock
            report.StatusStocks = vehicleList
                .GroupBy(v => v.Status)
                .Select(g => new StatusStockDto
                {
                    Status = g.Key,
                    Count = g.Count(),
                    TotalValue = g.Sum(v => v.PurchasePrice),
                    AveragePrice = g.Average(v => v.PurchasePrice)
                })
                .OrderByDescending(s => s.Count)
                .ToList();

            return report;
        }
    }
}
