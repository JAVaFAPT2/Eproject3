using MediatR;
using VehicleShowroomManagement.Application.Reports.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

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
        private readonly IRepository<User> _userRepository;

        public GetAllotmentDetailsReportQueryHandler(
            IRepository<Allotment> allotmentRepository,
            IRepository<Vehicle> vehicleRepository,
            IRepository<Customer> customerRepository,
            IRepository<User> userRepository)
        {
            _allotmentRepository = allotmentRepository;
            _vehicleRepository = vehicleRepository;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        public async Task<AllotmentDetailsReportDto> Handle(GetAllotmentDetailsReportQuery request, CancellationToken cancellationToken)
        {
            var allotments = await _allotmentRepository.GetAllAsync();
            var vehicles = await _vehicleRepository.GetAllAsync();
            var customers = await _customerRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                allotments = allotments.Where(a => 
                    a.AllotmentNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.SpecialConditions != null && a.SpecialConditions.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    a.Notes != null && a.Notes.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                allotments = allotments.Where(a => a.Status == request.Status);
            }

            if (!string.IsNullOrEmpty(request.AllotmentType))
            {
                allotments = allotments.Where(a => a.AllotmentType == request.AllotmentType);
            }

            if (request.FromDate.HasValue)
            {
                allotments = allotments.Where(a => a.AllotmentDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                allotments = allotments.Where(a => a.AllotmentDate <= request.ToDate.Value);
            }

            if (!request.IncludeExpired)
            {
                allotments = allotments.Where(a => !a.IsExpired());
            }

            if (!request.IncludeConverted)
            {
                allotments = allotments.Where(a => !a.ConvertedToOrder);
            }

            var allotmentList = allotments.ToList();
            var vehicleList = vehicles.ToList();
            var customerList = customers.ToList();
            var userList = users.ToList();

            var report = new AllotmentDetailsReportDto
            {
                GeneratedAt = DateTime.UtcNow,
                TotalAllotments = allotmentList.Count,
                ActiveAllotments = allotmentList.Count(a => a.IsActive()),
                ExpiredAllotments = allotmentList.Count(a => a.IsExpired()),
                ConvertedAllotments = allotmentList.Count(a => a.ConvertedToOrder),
                CancelledAllotments = allotmentList.Count(a => a.Status == "Cancelled"),
                TotalReservationAmount = allotmentList.Sum(a => a.ReservationAmount),
                CollectedReservationAmount = allotmentList.Where(a => a.ReservationPaid).Sum(a => a.ReservationAmount),
                PendingReservationAmount = allotmentList.Where(a => !a.ReservationPaid).Sum(a => a.ReservationAmount)
            };

            // Generate allotment details
            report.Allotments = allotmentList.Select(allotment =>
            {
                var vehicle = vehicleList.FirstOrDefault(v => v.Id == allotment.VehicleId);
                var customer = customerList.FirstOrDefault(c => c.Id == allotment.CustomerId);
                var salesPerson = userList.FirstOrDefault(u => u.Id == allotment.SalesPersonId);

                return new AllotmentDetailDto
                {
                    Id = allotment.Id,
                    AllotmentNumber = allotment.AllotmentNumber,
                    VehicleId = allotment.VehicleId,
                    VehicleName = vehicle?.Model?.Name ?? "N/A",
                    VehicleBrand = vehicle?.Model?.Brand ?? "N/A",
                    CustomerId = allotment.CustomerId,
                    CustomerName = customer != null ? $"{customer.FirstName} {customer.LastName}" : "N/A",
                    CustomerEmail = customer?.Email ?? "N/A",
                    CustomerPhone = customer?.Phone ?? "N/A",
                    SalesPersonId = allotment.SalesPersonId,
                    SalesPersonName = salesPerson != null ? $"{salesPerson.FirstName} {salesPerson.LastName}" : "N/A",
                    AllotmentDate = allotment.AllotmentDate,
                    ValidUntil = allotment.ValidUntil,
                    Status = allotment.Status,
                    AllotmentType = allotment.AllotmentType,
                    Priority = allotment.Priority,
                    ReservationAmount = allotment.ReservationAmount,
                    ReservationPaid = allotment.ReservationPaid,
                    PaymentMethod = allotment.PaymentMethod,
                    PaymentReference = allotment.PaymentReference,
                    SpecialConditions = allotment.SpecialConditions,
                    Notes = allotment.Notes,
                    ConvertedToOrder = allotment.ConvertedToOrder,
                    OrderId = allotment.OrderId,
                    ConversionDate = allotment.ConversionDate,
                    CancellationReason = allotment.CancellationReason,
                    CancelledDate = allotment.CancelledDate,
                    CancelledBy = allotment.CancelledBy,
                    CreatedBy = allotment.CreatedBy,
                    CreatedAt = allotment.CreatedAt,
                    UpdatedAt = allotment.UpdatedAt,
                    IsExpired = allotment.IsExpired(),
                    DaysRemaining = allotment.IsExpired() ? 0 : Math.Max(0, (int)(allotment.ValidUntil - DateTime.UtcNow).TotalDays)
                };
            }).OrderByDescending(a => a.AllotmentDate).ToList();

            // Generate status summaries
            var totalAllotments = allotmentList.Count;
            report.StatusSummaries = allotmentList
                .GroupBy(a => a.Status)
                .Select(g => new StatusSummaryDto
                {
                    Status = g.Key,
                    Count = g.Count(),
                    TotalReservationAmount = g.Sum(a => a.ReservationAmount),
                    CollectedAmount = g.Where(a => a.ReservationPaid).Sum(a => a.ReservationAmount),
                    PendingAmount = g.Where(a => !a.ReservationPaid).Sum(a => a.ReservationAmount),
                    Percentage = totalAllotments > 0 ? (decimal)g.Count() / totalAllotments * 100 : 0
                })
                .OrderByDescending(s => s.Count)
                .ToList();

            // Generate type summaries
            report.TypeSummaries = allotmentList
                .GroupBy(a => a.AllotmentType)
                .Select(g => new TypeSummaryDto
                {
                    AllotmentType = g.Key,
                    Count = g.Count(),
                    TotalReservationAmount = g.Sum(a => a.ReservationAmount),
                    CollectedAmount = g.Where(a => a.ReservationPaid).Sum(a => a.ReservationAmount),
                    PendingAmount = g.Where(a => !a.ReservationPaid).Sum(a => a.ReservationAmount),
                    AverageReservationAmount = g.Average(a => a.ReservationAmount)
                })
                .OrderByDescending(t => t.Count)
                .ToList();

            // Generate monthly trends
            report.MonthlyTrends = allotmentList
                .GroupBy(a => new { Year = a.AllotmentDate.Year, Month = a.AllotmentDate.Month })
                .Select(g => new MonthlyAllotmentDto
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    NewAllotments = g.Count(),
                    ConvertedAllotments = g.Count(a => a.ConvertedToOrder),
                    ExpiredAllotments = g.Count(a => a.IsExpired()),
                    CancelledAllotments = g.Count(a => a.Status == "Cancelled"),
                    TotalReservationAmount = g.Sum(a => a.ReservationAmount),
                    CollectedAmount = g.Where(a => a.ReservationPaid).Sum(a => a.ReservationAmount),
                    ConversionRate = g.Count() > 0 ? (decimal)g.Count(a => a.ConvertedToOrder) / g.Count() * 100 : 0
                })
                .OrderBy(m => m.Month)
                .ToList();

            return report;
        }
    }
}
