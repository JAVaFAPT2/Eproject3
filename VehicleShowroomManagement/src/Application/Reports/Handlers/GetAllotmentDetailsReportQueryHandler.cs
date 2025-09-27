using MediatR;
using VehicleShowroomManagement.Application.Reports.DTOs;
using VehicleShowroomManagement.Application.Reports.Queries;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Reports.Handlers
{
    /// <summary>
    /// Handler for getting allotment details report
    /// </summary>
    public class GetAllotmentDetailsReportQueryHandler : IRequestHandler<GetAllotmentDetailsReportQuery, AllotmentDetailsReportDto>
    {
        private readonly IRepository<Allotment> _allotmentRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public GetAllotmentDetailsReportQueryHandler(
            IRepository<Allotment> allotmentRepository,
            IRepository<Vehicle> vehicleRepository,
            IRepository<Customer> customerRepository,
            IRepository<Employee> employeeRepository)
        {
            _allotmentRepository = allotmentRepository;
            _vehicleRepository = vehicleRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<AllotmentDetailsReportDto> Handle(GetAllotmentDetailsReportQuery request, CancellationToken cancellationToken)
        {
            var allotments = await _allotmentRepository.GetAllAsync();
            var vehicles = await _vehicleRepository.GetAllAsync();
            var customers = await _customerRepository.GetAllAsync();
            var employees = await _employeeRepository.GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                allotments = allotments.Where(a => 
                    a.AllotmentId.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (request.FromDate.HasValue)
            {
                allotments = allotments.Where(a => a.AllotmentDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                allotments = allotments.Where(a => a.AllotmentDate <= request.ToDate.Value);
            }

            var allotmentList = allotments.ToList();
            var vehicleList = vehicles.ToList();
            var customerList = customers.ToList();
            var employeeList = employees.ToList();

            var report = new AllotmentDetailsReportDto
            {
                GeneratedAt = DateTime.UtcNow,
                TotalAllotments = allotmentList.Count,
                ActiveAllotments = allotmentList.Count(a => !a.IsDeleted),
                ExpiredAllotments = 0, // Not available in new schema
                ConvertedAllotments = 0, // Not available in new schema
                CancelledAllotments = 0, // Not available in new schema
                TotalReservationAmount = 0, // Not available in new schema
                CollectedReservationAmount = 0, // Not available in new schema
                PendingReservationAmount = 0 // Not available in new schema
            };

            // Generate allotment details
            report.Allotments = allotmentList.Select(allotment =>
            {
                var vehicle = vehicleList.FirstOrDefault(v => v.Id == allotment.VehicleId);
                var customer = customerList.FirstOrDefault(c => c.Id == allotment.CustomerId);
                var employee = employeeList.FirstOrDefault(e => e.Id == allotment.EmployeeId);

                return new AllotmentDetailDto
                {
                    Id = allotment.Id,
                    AllotmentNumber = allotment.AllotmentId,
                    VehicleId = allotment.VehicleId,
                    VehicleName = vehicle?.ModelNumber ?? "N/A",
                    VehicleBrand = "N/A", // Not available in new schema
                    CustomerId = allotment.CustomerId,
                    CustomerName = customer?.Name ?? "N/A",
                    CustomerEmail = customer?.Email ?? "N/A",
                    CustomerPhone = customer?.Phone ?? "N/A",
                    SalesPersonId = allotment.EmployeeId,
                    SalesPersonName = employee?.Name ?? "N/A",
                    AllotmentDate = allotment.AllotmentDate,
                    ValidUntil = DateTime.UtcNow.AddDays(30), // Default value
                    Status = "Active", // Default value
                    AllotmentType = "Standard", // Default value
                    Priority = 1, // Default value
                    ReservationAmount = 0, // Not available in new schema
                    ReservationPaid = false, // Not available in new schema
                    PaymentMethod = "N/A", // Not available in new schema
                    PaymentReference = "N/A", // Not available in new schema
                    SpecialConditions = "N/A", // Not available in new schema
                    Notes = "N/A", // Not available in new schema
                    ConvertedToOrder = false, // Not available in new schema
                    OrderId = "N/A", // Not available in new schema
                    ConversionDate = null, // Not available in new schema
                    CancellationReason = "N/A", // Not available in new schema
                    CancelledDate = null, // Not available in new schema
                    CancelledBy = "N/A", // Not available in new schema
                    CreatedBy = "N/A", // Not available in new schema
                    CreatedAt = allotment.CreatedAt,
                    UpdatedAt = allotment.UpdatedAt,
                    IsExpired = false, // Default value
                    DaysRemaining = 30 // Default value
                };
            }).OrderByDescending(a => a.AllotmentDate).ToList();

            // Generate status summaries (simplified for new schema)
            var totalAllotments = allotmentList.Count;
            report.StatusSummaries = new List<StatusSummaryDto>
            {
                new StatusSummaryDto
                {
                    Status = "Active",
                    Count = totalAllotments,
                    TotalReservationAmount = 0,
                    CollectedAmount = 0,
                    PendingAmount = 0,
                    Percentage = 100
                }
            };

            // Generate type summaries (simplified for new schema)
            report.TypeSummaries = new List<TypeSummaryDto>
            {
                new TypeSummaryDto
                {
                    AllotmentType = "Standard",
                    Count = totalAllotments,
                    TotalReservationAmount = 0,
                    CollectedAmount = 0,
                    PendingAmount = 0,
                    AverageReservationAmount = 0
                }
            };

            // Generate monthly trends (simplified for new schema)
            report.MonthlyTrends = allotmentList
                .GroupBy(a => new { Year = a.AllotmentDate.Year, Month = a.AllotmentDate.Month })
                .Select(g => new MonthlyAllotmentDto
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    NewAllotments = g.Count(),
                    ConvertedAllotments = 0,
                    ExpiredAllotments = 0,
                    CancelledAllotments = 0,
                    TotalReservationAmount = 0,
                    CollectedAmount = 0,
                    ConversionRate = 0
                })
                .OrderBy(m => m.Month)
                .ToList();

            return report;
        }
    }
}
