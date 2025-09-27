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

            // Apply filters
            if (!string.IsNullOrEmpty(request.Brand))
            {
                vehicles = vehicles.Where(v => v.Model?.Brand?.BrandName == request.Brand);
            }

            if (!string.IsNullOrEmpty(request.Model))
            {
                vehicles = vehicles.Where(v => v.Model?.ModelName == request.Model);
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                vehicles = vehicles.Where(v => v.Status == request.Status);
            }

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
                TotalValue = vehicleList.Sum(v => v.Price),
                AvailableValue = vehicleList.Where(v => v.Status == "Available").Sum(v => v.Price)
            };

            // Generate brand-wise stock
            report.BrandStocks = vehicleList
                .GroupBy(v => v.Model?.Brand?.BrandName ?? "Unknown")
                .Select(g => new BrandStockDto
                {
                    Brand = g.Key,
                    TotalCount = g.Count(),
                    AvailableCount = g.Count(v => v.Status == "Available"),
                    SoldCount = g.Count(v => v.Status == "Sold"),
                    ReservedCount = g.Count(v => v.Status == "Reserved"),
                    InServiceCount = g.Count(v => v.Status == "InService"),
                    TotalValue = g.Sum(v => v.Price),
                    AvailableValue = g.Where(v => v.Status == "Available").Sum(v => v.Price)
                })
                .OrderByDescending(b => b.TotalCount)
                .ToList();

            // Generate model-wise stock
            report.ModelStocks = vehicleList
                .GroupBy(v => new { Brand = v.Model?.Brand?.BrandName ?? "Unknown", Model = v.Model?.ModelName ?? "Unknown" })
                .Select(g => new ModelStockDto
                {
                    Brand = g.Key.Brand,
                    Model = g.Key.Model,
                    TotalCount = g.Count(),
                    AvailableCount = g.Count(v => v.Status == "Available"),
                    SoldCount = g.Count(v => v.Status == "Sold"),
                    ReservedCount = g.Count(v => v.Status == "Reserved"),
                    InServiceCount = g.Count(v => v.Status == "InService"),
                    TotalValue = g.Sum(v => v.Price),
                    AvailableValue = g.Where(v => v.Status == "Available").Sum(v => v.Price),
                    AveragePrice = g.Average(v => v.Price)
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
                    TotalValue = g.Sum(v => v.Price),
                    AveragePrice = g.Average(v => v.Price)
                })
                .OrderByDescending(s => s.Count)
                .ToList();

            return report;
        }
    }
}
