using MediatR;
using VehicleShowroomManagement.Application.Reports.DTOs;
using VehicleShowroomManagement.Application.Reports.Queries;
using VehicleShowroomManagement.Domain.Entities;

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

            // Apply filters (simplified for new schema)
            if (!string.IsNullOrEmpty(request.Status))
            {
                vehicles = vehicles.Where(v => v.Status == request.Status);
            }

            // Brand and Model filtering not directly available in new schema
            // Price filtering uses PurchasePrice instead of Price

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
                TotalValue = vehicleList.Sum(v => v.PurchasePrice),
                AveragePrice = vehicleList.Any() ? vehicleList.Average(v => v.PurchasePrice) : 0
            };

            // Generate vehicle details
            report.Vehicles = vehicleList.Select(vehicle =>
            {
                var registration = request.IncludeRegistrationInfo 
                    ? vehicleRegistrationList.FirstOrDefault(vr => vr.VehicleId == vehicle.Id)
                    : null;

                var vehicleServiceOrders = request.IncludeServiceHistory
                    ? serviceOrderList.Where(so => so.SalesOrderId == vehicle.Id).ToList()
                    : new List<ServiceOrder>();

                return new VehicleDetailDto
                {
                    Id = vehicle.Id,
                    VIN = vehicle.VehicleId,
                    ModelNumber = vehicle.ModelNumber,
                    Name = vehicle.ModelNumber,
                    Brand = "Unknown", // Not available in new schema
                    Price = vehicle.PurchasePrice,
                    Status = vehicle.Status,
                    Year = 0, // Not available in new schema
                    Color = "Unknown", // Not available in new schema
                    Mileage = 0, // Not available in new schema
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
                        ServiceOrderId = so.ServiceOrderId,
                        SalesOrderId = so.SalesOrderId,
                        EmployeeId = so.EmployeeId,
                        ServiceDate = so.ServiceDate,
                        Description = so.Description,
                        Cost = so.Cost
                    }).OrderByDescending(so => so.ServiceDate).ToList()
                };
            }).OrderBy(v => v.Brand).ThenBy(v => v.Name).ToList();

            // Generate brand summaries (simplified for new schema)
            report.BrandSummaries = new List<BrandSummaryDto>
            {
                new BrandSummaryDto
                {
                    Brand = "Unknown",
                    VehicleCount = vehicleList.Count,
                    TotalValue = vehicleList.Sum(v => v.PurchasePrice),
                    AveragePrice = vehicleList.Average(v => v.PurchasePrice),
                    AvailableCount = vehicleList.Count(v => v.Status == "Available"),
                    SoldCount = vehicleList.Count(v => v.Status == "Sold"),
                    ReservedCount = vehicleList.Count(v => v.Status == "Reserved"),
                    InServiceCount = vehicleList.Count(v => v.Status == "InService")
                }
            };

            // Generate model summaries (simplified for new schema)
            report.ModelSummaries = vehicleList
                .GroupBy(v => v.ModelNumber)
                .Select(g => new ModelSummaryDto
                {
                    Brand = "Unknown",
                    Model = g.Key,
                    VehicleCount = g.Count(),
                    TotalValue = g.Sum(v => v.PurchasePrice),
                    AveragePrice = g.Average(v => v.PurchasePrice),
                    MinPrice = g.Min(v => v.PurchasePrice),
                    MaxPrice = g.Max(v => v.PurchasePrice),
                    AvailableCount = g.Count(v => v.Status == "Available"),
                    SoldCount = g.Count(v => v.Status == "Sold")
                })
                .OrderByDescending(m => m.VehicleCount)
                .ToList();

            // Generate status summaries
            var totalVehicles = vehicleList.Count;
            report.StatusSummaries = vehicleList
                .GroupBy(v => v.Status)
                .Select(g => new VehicleStatusSummaryDto
                {
                    Status = g.Key,
                    VehicleCount = g.Count(),
                    TotalValue = g.Sum(v => v.PurchasePrice),
                    AveragePrice = g.Average(v => v.PurchasePrice),
                    Percentage = totalVehicles > 0 ? (decimal)g.Count() / totalVehicles * 100 : 0
                })
                .OrderByDescending(s => s.VehicleCount)
                .ToList();

            // Generate year summaries
            report.YearSummaries = vehicleList
                .GroupBy(v => 0)
                .Select(g => new YearSummaryDto
                {
                    Year = g.Key,
                    VehicleCount = g.Count(),
                    TotalValue = g.Sum(v => v.PurchasePrice),
                    AveragePrice = g.Average(v => v.PurchasePrice),
                    AvailableCount = g.Count(v => v.Status == "Available"),
                    SoldCount = g.Count(v => v.Status == "Sold")
                })
                .OrderByDescending(y => y.Year)
                .ToList();

            return report;
        }
    }
}
