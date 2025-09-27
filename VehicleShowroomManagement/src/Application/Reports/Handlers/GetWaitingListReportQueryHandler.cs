using MediatR;
using VehicleShowroomManagement.Application.Reports.DTOs;
using VehicleShowroomManagement.Application.Reports.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.Reports.Handlers
{
    /// <summary>
    /// Handler for getting waiting list details report
    /// </summary>
    public class GetWaitingListReportQueryHandler : IRequestHandler<GetWaitingListReportQuery, WaitingListReportDto>
    {
        private readonly IRepository<WaitingList> _waitingListRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public GetWaitingListReportQueryHandler(
            IRepository<WaitingList> waitingListRepository,
            IRepository<Customer> customerRepository,
            IRepository<Employee> employeeRepository)
        {
            _waitingListRepository = waitingListRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<WaitingListReportDto> Handle(GetWaitingListReportQuery request, CancellationToken cancellationToken)
        {
            var waitingLists = await _waitingListRepository.GetAllAsync();
            var customers = await _customerRepository.GetAllAsync();
            var employees = await _employeeRepository.GetAllAsync();

            // Apply filters (simplified for new schema)
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                waitingLists = waitingLists.Where(wl => 
                    wl.WaitId.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                waitingLists = waitingLists.Where(wl => wl.Status == request.Status);
            }

            if (request.FromDate.HasValue)
            {
                waitingLists = waitingLists.Where(wl => wl.RequestDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                waitingLists = waitingLists.Where(wl => wl.RequestDate <= request.ToDate.Value);
            }

            var waitingListEntries = waitingLists.ToList();
            var customerList = customers.ToList();
            var employeeList = employees.ToList();

            var report = new WaitingListReportDto
            {
                GeneratedAt = DateTime.UtcNow,
                TotalWaitingListEntries = waitingListEntries.Count,
                ActiveEntries = waitingListEntries.Count(wl => !wl.IsDeleted),
                NotifiedEntries = 0, // Not available in new schema
                ConvertedEntries = 0, // Not available in new schema
                CancelledEntries = 0, // Not available in new schema
                ExpiredEntries = 0, // Not available in new schema
                HighPriorityEntries = 0, // Not available in new schema
                MediumPriorityEntries = 0, // Not available in new schema
                LowPriorityEntries = 0 // Not available in new schema
            };

            // Generate waiting list details (simplified for new schema)
            report.WaitingListEntries = waitingListEntries.Select(waitingList =>
            {
                var customer = customerList.FirstOrDefault(c => c.Id == waitingList.CustomerId);

                return new WaitingListDetailDto
                {
                    Id = waitingList.Id,
                    WaitingListNumber = waitingList.WaitId,
                    CustomerId = waitingList.CustomerId,
                    CustomerName = customer?.Name ?? "N/A",
                    CustomerEmail = customer?.Email ?? "N/A",
                    CustomerPhone = customer?.Phone ?? "N/A",
                    ModelId = waitingList.ModelNumber,
                    Brand = "N/A", // Not available in new schema
                    ModelName = waitingList.ModelNumber,
                    VehicleType = "N/A", // Not available in new schema
                    PreferredColor = "N/A", // Not available in new schema
                    PreferredYear = 0, // Not available in new schema
                    MaxPrice = 0, // Not available in new schema
                    MinPrice = 0, // Not available in new schema
                    Priority = 1, // Default value
                    Position = 1, // Default value
                    Status = waitingList.Status,
                    RequestDate = waitingList.RequestDate,
                    ExpectedAvailabilityDate = DateTime.UtcNow.AddDays(30), // Default value
                    LastContactDate = DateTime.UtcNow, // Default value
                    NextContactDate = DateTime.UtcNow.AddDays(7), // Default value
                    ContactMethod = "N/A", // Not available in new schema
                    ContactFrequency = "N/A", // Not available in new schema
                    IsFlexible = false, // Default value
                    FlexibilityNotes = "N/A", // Not available in new schema
                    SpecialRequirements = "N/A", // Not available in new schema
                    Notes = "N/A", // Not available in new schema
                    ConvertedToAllotment = false, // Not available in new schema
                    AllotmentId = "N/A", // Not available in new schema
                    ConversionDate = null, // Not available in new schema
                    CancellationReason = "N/A", // Not available in new schema
                    CancelledDate = null, // Not available in new schema
                    CancelledBy = "N/A", // Not available in new schema
                    AssignedTo = "N/A", // Not available in new schema
                    AssignedToName = "N/A", // Not available in new schema
                    CreatedBy = "N/A", // Not available in new schema
                    CreatedAt = waitingList.CreatedAt,
                    UpdatedAt = waitingList.UpdatedAt,
                    DaysWaiting = (int)(DateTime.UtcNow - waitingList.RequestDate).TotalDays,
                    IsEligibleForNotification = !waitingList.IsDeleted
                };
            }).OrderBy(wl => wl.RequestDate).ToList();

            // Generate brand-wise waiting data (simplified for new schema)
            report.BrandWaiting = new List<BrandWaitingDto>();

            // Generate model-wise waiting data (simplified for new schema)
            report.ModelWaiting = new List<ModelWaitingDto>();

            // Generate priority-wise waiting data (simplified for new schema)
            report.PriorityWaiting = new List<PriorityWaitingDto>();

            // Generate monthly trends (simplified for new schema)
            report.MonthlyTrends = waitingListEntries
                .GroupBy(wl => new { Year = wl.RequestDate.Year, Month = wl.RequestDate.Month })
                .Select(g => new MonthlyWaitingDto
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    NewEntries = g.Count(),
                    ConvertedEntries = 0,
                    CancelledEntries = 0,
                    ExpiredEntries = 0,
                    ActiveEntries = g.Count(wl => !wl.IsDeleted),
                    ConversionRate = 0,
                    AverageWaitingDays = (decimal)g.Average(wl => (DateTime.UtcNow - wl.RequestDate).TotalDays)
                })
                .OrderBy(m => m.Month)
                .ToList();

            return report;
        }
    }
}
