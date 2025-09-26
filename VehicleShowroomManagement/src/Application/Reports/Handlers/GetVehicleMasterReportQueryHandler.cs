using MediatR;
using VehicleShowroomManagement.Application.Reports.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.Reports.Handlers
{
    /// <summary>
    /// Handler for getting vehicle master information report
    /// </summary>
    public class GetVehicleMasterReportQueryHandler : IRequestHandler<GetVehicleMasterReportQuery, VehicleMasterReportDto>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<VehicleRegistration> _vehicleRegistrationRepository;
        private readonly IRepository<ServiceOrder> _serviceOrderRepository;

        public GetVehicleMasterReportQueryHandler(
            IRepository<Vehicle> vehicleRepository,
            IRepository<VehicleRegistration> vehicleRegistrationRepository,
            IRepository<ServiceOrder> serviceOrderRepository)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
            _serviceOrderRepository = serviceOrderRepository;
        }

        public async Task<VehicleMasterReportDto> Handle(GetVehicleMasterReportQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            var vehicleRegistrations = await _vehicleRegistrationRepository.GetAllAsync();
            var serviceOrders = await _serviceOrderRepository.GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(request.Brand))
            {
                vehicles = vehicles.Where(v => v.Model?.Brand == request.Brand);
            }

            if (!string.IsNullOrEmpty(request.Model))
            {
                vehicles = vehicles.Where(v => v.Model?.Name == request.Model);
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                vehicles = vehicles.Where(v => v.Status == request.Status);
            }

            if (request.Year.HasValue)
            {
                vehicles = vehicles.Where(v => v.Year == request.Year.Value);
            }

            if (!string.IsNullOrEmpty(request.Color))
            {
                vehicles = vehicles.Where(v => v.Color == request.Color);
            }

            if (request.MinPrice.HasValue)
            {
                vehicles = vehicles.Where(v => v.Price >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                vehicles = vehicles.Where(v => v.Price <= request.MaxPrice.Value);
            }

            if (request.FromDate.HasValue)
            {
                vehicles = vehicles.Where(v => v.CreatedAt >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                vehicles = vehicles.Where(v => v.CreatedAt <= request.ToDate.Value);
            }

            var vehicleList = vehicles.ToList();
            var vehicleRegistrationList = vehicleRegistrations.ToList();
            var serviceOrderList = serviceOrders.ToList();

            var report = new VehicleMasterReportDto
            {
                GeneratedAt = DateTime.UtcNow,
                TotalVehicles = vehicleList.Count,
                TotalValue = vehicleList.Sum(v => v.Price),
                AveragePrice = vehicleList.Any() ? vehicleList.Average(v => v.Price) : 0
            };

            // Generate vehicle details
            report.Vehicles = vehicleList.Select(vehicle =>
            {
                var registration = request.IncludeRegistrationInfo 
                    ? vehicleRegistrationList.FirstOrDefault(vr => vr.VehicleId == vehicle.Id)
                    : null;

                var vehicleServiceOrders = request.IncludeServiceHistory 
                    ? serviceOrderList.Where(so => so.VehicleId == vehicle.Id).ToList()
                    : new List<ServiceOrder>();

                return new VehicleDetailDto
                {
                    Id = vehicle.Id,
                    VIN = vehicle.VIN,
                    ModelNumber = vehicle.Model?.ModelNumber ?? "N/A",
                    Name = vehicle.Model?.Name ?? "N/A",
                    Brand = vehicle.Model?.Brand ?? "N/A",
                    Price = vehicle.Price,
                    Status = vehicle.Status,
                    Year = vehicle.Year,
                    Color = vehicle.Color,
                    Mileage = vehicle.Mileage,
                    CreatedAt = vehicle.CreatedAt,
                    UpdatedAt = vehicle.UpdatedAt,
                    Registration = registration != null ? new VehicleRegistrationDto
                    {
                        RegistrationNumber = registration.RegistrationNumber,
                        RegistrationDate = registration.RegistrationDate,
                        ExpiryDate = registration.ExpiryDate,
                        RegistrationState = registration.RegistrationState,
                        RegistrationCity = registration.RegistrationCity,
                        OwnerName = registration.OwnerName,
                        VehicleType = registration.VehicleType,
                        FuelType = registration.FuelType,
                        Status = registration.Status
                    } : null,
                    ServiceHistory = vehicleServiceOrders.Select(so => new ServiceOrderDto
                    {
                        Id = so.Id,
                        ServiceDate = so.ServiceDate,
                        ServiceType = so.ServiceType,
                        Status = so.Status,
                        TotalCost = so.TotalCost,
                        Description = so.Description
                    }).OrderByDescending(so => so.ServiceDate).ToList()
                };
            }).OrderBy(v => v.Brand).ThenBy(v => v.Name).ToList();

            // Generate brand summaries
            report.BrandSummaries = vehicleList
                .GroupBy(v => v.Model?.Brand ?? "Unknown")
                .Select(g => new BrandSummaryDto
                {
                    Brand = g.Key,
                    VehicleCount = g.Count(),
                    TotalValue = g.Sum(v => v.Price),
                    AveragePrice = g.Average(v => v.Price),
                    AvailableCount = g.Count(v => v.Status == "Available"),
                    SoldCount = g.Count(v => v.Status == "Sold"),
                    ReservedCount = g.Count(v => v.Status == "Reserved"),
                    InServiceCount = g.Count(v => v.Status == "InService")
                })
                .OrderByDescending(b => b.VehicleCount)
                .ToList();

            // Generate model summaries
            report.ModelSummaries = vehicleList
                .GroupBy(v => new { Brand = v.Model?.Brand ?? "Unknown", Model = v.Model?.Name ?? "Unknown" })
                .Select(g => new ModelSummaryDto
                {
                    Brand = g.Key.Brand,
                    Model = g.Key.Model,
                    VehicleCount = g.Count(),
                    TotalValue = g.Sum(v => v.Price),
                    AveragePrice = g.Average(v => v.Price),
                    MinPrice = g.Min(v => v.Price),
                    MaxPrice = g.Max(v => v.Price),
                    AvailableCount = g.Count(v => v.Status == "Available"),
                    SoldCount = g.Count(v => v.Status == "Sold")
                })
                .OrderByDescending(m => m.VehicleCount)
                .ToList();

            // Generate status summaries
            var totalVehicles = vehicleList.Count;
            report.StatusSummaries = vehicleList
                .GroupBy(v => v.Status)
                .Select(g => new StatusSummaryDto
                {
                    Status = g.Key,
                    VehicleCount = g.Count(),
                    TotalValue = g.Sum(v => v.Price),
                    AveragePrice = g.Average(v => v.Price),
                    Percentage = totalVehicles > 0 ? (decimal)g.Count() / totalVehicles * 100 : 0
                })
                .OrderByDescending(s => s.VehicleCount)
                .ToList();

            // Generate year summaries
            report.YearSummaries = vehicleList
                .GroupBy(v => v.Year)
                .Select(g => new YearSummaryDto
                {
                    Year = g.Key,
                    VehicleCount = g.Count(),
                    TotalValue = g.Sum(v => v.Price),
                    AveragePrice = g.Average(v => v.Price),
                    AvailableCount = g.Count(v => v.Status == "Available"),
                    SoldCount = g.Count(v => v.Status == "Sold")
                })
                .OrderByDescending(y => y.Year)
                .ToList();

            return report;
        }
    }
}
