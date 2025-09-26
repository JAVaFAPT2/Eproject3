using MediatR;
using VehicleShowroomManagement.Application.Reports.DTOs;
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
        private readonly IRepository<User> _userRepository;

        public GetWaitingListReportQueryHandler(
            IRepository<WaitingList> waitingListRepository,
            IRepository<Customer> customerRepository,
            IRepository<User> userRepository)
        {
            _waitingListRepository = waitingListRepository;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        public async Task<WaitingListReportDto> Handle(GetWaitingListReportQuery request, CancellationToken cancellationToken)
        {
            var waitingLists = await _waitingListRepository.GetAllAsync();
            var customers = await _customerRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                waitingLists = waitingLists.Where(wl => 
                    wl.WaitingListNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    wl.SpecialRequirements != null && wl.SpecialRequirements.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    wl.Notes != null && wl.Notes.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    wl.FlexibilityNotes != null && wl.FlexibilityNotes.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                waitingLists = waitingLists.Where(wl => wl.Status == request.Status);
            }

            if (!string.IsNullOrEmpty(request.Brand))
            {
                waitingLists = waitingLists.Where(wl => wl.Brand == request.Brand);
            }

            if (!string.IsNullOrEmpty(request.Model))
            {
                waitingLists = waitingLists.Where(wl => wl.ModelName == request.Model);
            }

            if (!string.IsNullOrEmpty(request.VehicleType))
            {
                waitingLists = waitingLists.Where(wl => wl.VehicleType == request.VehicleType);
            }

            if (request.Priority.HasValue)
            {
                waitingLists = waitingLists.Where(wl => wl.Priority == request.Priority.Value);
            }

            if (request.FromDate.HasValue)
            {
                waitingLists = waitingLists.Where(wl => wl.EntryDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                waitingLists = waitingLists.Where(wl => wl.EntryDate <= request.ToDate.Value);
            }

            if (!request.IncludeExpired)
            {
                waitingLists = waitingLists.Where(wl => wl.Status != "Expired");
            }

            if (!request.IncludeConverted)
            {
                waitingLists = waitingLists.Where(wl => !wl.ConvertedToAllotment);
            }

            var waitingListEntries = waitingLists.ToList();
            var customerList = customers.ToList();
            var userList = users.ToList();

            var report = new WaitingListReportDto
            {
                GeneratedAt = DateTime.UtcNow,
                TotalWaitingListEntries = waitingListEntries.Count,
                ActiveEntries = waitingListEntries.Count(wl => wl.IsActive()),
                NotifiedEntries = waitingListEntries.Count(wl => wl.Status == "Notified"),
                ConvertedEntries = waitingListEntries.Count(wl => wl.ConvertedToAllotment),
                CancelledEntries = waitingListEntries.Count(wl => wl.Status == "Cancelled"),
                ExpiredEntries = waitingListEntries.Count(wl => wl.Status == "Expired"),
                HighPriorityEntries = waitingListEntries.Count(wl => wl.Priority == 1),
                MediumPriorityEntries = waitingListEntries.Count(wl => wl.Priority == 2),
                LowPriorityEntries = waitingListEntries.Count(wl => wl.Priority == 3)
            };

            // Generate waiting list details
            report.WaitingListEntries = waitingListEntries.Select(waitingList =>
            {
                var customer = customerList.FirstOrDefault(c => c.Id == waitingList.CustomerId);
                var assignedUser = !string.IsNullOrEmpty(waitingList.AssignedTo) 
                    ? userList.FirstOrDefault(u => u.Id == waitingList.AssignedTo) 
                    : null;

                return new WaitingListDetailDto
                {
                    Id = waitingList.Id,
                    WaitingListNumber = waitingList.WaitingListNumber,
                    CustomerId = waitingList.CustomerId,
                    CustomerName = customer != null ? $"{customer.FirstName} {customer.LastName}" : "N/A",
                    CustomerEmail = customer?.Email ?? "N/A",
                    CustomerPhone = customer?.Phone ?? "N/A",
                    ModelId = waitingList.ModelId,
                    Brand = waitingList.Brand,
                    ModelName = waitingList.ModelName,
                    VehicleType = waitingList.VehicleType,
                    PreferredColor = waitingList.PreferredColor,
                    PreferredYear = waitingList.PreferredYear,
                    MaxPrice = waitingList.MaxPrice,
                    MinPrice = waitingList.MinPrice,
                    Priority = waitingList.Priority,
                    Position = waitingList.Position,
                    Status = waitingList.Status,
                    EntryDate = waitingList.EntryDate,
                    ExpectedAvailabilityDate = waitingList.ExpectedAvailabilityDate,
                    LastContactDate = waitingList.LastContactDate,
                    NextContactDate = waitingList.NextContactDate,
                    ContactMethod = waitingList.ContactMethod,
                    ContactFrequency = waitingList.ContactFrequency,
                    IsFlexible = waitingList.IsFlexible,
                    FlexibilityNotes = waitingList.FlexibilityNotes,
                    SpecialRequirements = waitingList.SpecialRequirements,
                    Notes = waitingList.Notes,
                    ConvertedToAllotment = waitingList.ConvertedToAllotment,
                    AllotmentId = waitingList.AllotmentId,
                    ConversionDate = waitingList.ConversionDate,
                    CancellationReason = waitingList.CancellationReason,
                    CancelledDate = waitingList.CancelledDate,
                    CancelledBy = waitingList.CancelledBy,
                    AssignedTo = waitingList.AssignedTo,
                    AssignedToName = assignedUser != null ? $"{assignedUser.FirstName} {assignedUser.LastName}" : "N/A",
                    CreatedBy = waitingList.CreatedBy,
                    CreatedAt = waitingList.CreatedAt,
                    UpdatedAt = waitingList.UpdatedAt,
                    DaysWaiting = (int)(DateTime.UtcNow - waitingList.EntryDate).TotalDays,
                    IsEligibleForNotification = waitingList.IsActive() && 
                        (waitingList.NextContactDate == null || waitingList.NextContactDate <= DateTime.UtcNow)
                };
            }).OrderBy(wl => wl.Priority).ThenBy(wl => wl.Position).ToList();

            // Generate brand-wise waiting data
            report.BrandWaiting = waitingListEntries
                .Where(wl => !string.IsNullOrEmpty(wl.Brand))
                .GroupBy(wl => wl.Brand)
                .Select(g => new BrandWaitingDto
                {
                    Brand = g.Key,
                    TotalEntries = g.Count(),
                    ActiveEntries = g.Count(wl => wl.IsActive()),
                    HighPriorityEntries = g.Count(wl => wl.Priority == 1),
                    MediumPriorityEntries = g.Count(wl => wl.Priority == 2),
                    LowPriorityEntries = g.Count(wl => wl.Priority == 3),
                    ConvertedEntries = g.Count(wl => wl.ConvertedToAllotment),
                    AverageWaitingDays = g.Average(wl => (DateTime.UtcNow - wl.EntryDate).TotalDays),
                    ConversionRate = g.Count() > 0 ? (decimal)g.Count(wl => wl.ConvertedToAllotment) / g.Count() * 100 : 0
                })
                .OrderByDescending(b => b.TotalEntries)
                .ToList();

            // Generate model-wise waiting data
            report.ModelWaiting = waitingListEntries
                .Where(wl => !string.IsNullOrEmpty(wl.Brand) && !string.IsNullOrEmpty(wl.ModelName))
                .GroupBy(wl => new { Brand = wl.Brand, Model = wl.ModelName })
                .Select(g => new ModelWaitingDto
                {
                    Brand = g.Key.Brand,
                    Model = g.Key.Model,
                    TotalEntries = g.Count(),
                    ActiveEntries = g.Count(wl => wl.IsActive()),
                    HighPriorityEntries = g.Count(wl => wl.Priority == 1),
                    MediumPriorityEntries = g.Count(wl => wl.Priority == 2),
                    LowPriorityEntries = g.Count(wl => wl.Priority == 3),
                    ConvertedEntries = g.Count(wl => wl.ConvertedToAllotment),
                    AverageWaitingDays = g.Average(wl => (DateTime.UtcNow - wl.EntryDate).TotalDays),
                    ConversionRate = g.Count() > 0 ? (decimal)g.Count(wl => wl.ConvertedToAllotment) / g.Count() * 100 : 0,
                    AverageMaxPrice = g.Where(wl => wl.MaxPrice.HasValue).Average(wl => wl.MaxPrice.Value),
                    AverageMinPrice = g.Where(wl => wl.MinPrice.HasValue).Average(wl => wl.MinPrice.Value)
                })
                .OrderByDescending(m => m.TotalEntries)
                .ToList();

            // Generate priority-wise waiting data
            report.PriorityWaiting = waitingListEntries
                .GroupBy(wl => wl.Priority)
                .Select(g => new PriorityWaitingDto
                {
                    Priority = g.Key,
                    PriorityName = g.Key switch
                    {
                        1 => "High",
                        2 => "Medium",
                        3 => "Low",
                        _ => "Unknown"
                    },
                    TotalEntries = g.Count(),
                    ActiveEntries = g.Count(wl => wl.IsActive()),
                    ConvertedEntries = g.Count(wl => wl.ConvertedToAllotment),
                    AverageWaitingDays = g.Average(wl => (DateTime.UtcNow - wl.EntryDate).TotalDays),
                    ConversionRate = g.Count() > 0 ? (decimal)g.Count(wl => wl.ConvertedToAllotment) / g.Count() * 100 : 0,
                    AverageMaxPrice = g.Where(wl => wl.MaxPrice.HasValue).Average(wl => wl.MaxPrice.Value),
                    AverageMinPrice = g.Where(wl => wl.MinPrice.HasValue).Average(wl => wl.MinPrice.Value)
                })
                .OrderBy(p => p.Priority)
                .ToList();

            // Generate monthly trends
            report.MonthlyTrends = waitingListEntries
                .GroupBy(wl => new { Year = wl.EntryDate.Year, Month = wl.EntryDate.Month })
                .Select(g => new MonthlyWaitingDto
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    NewEntries = g.Count(),
                    ConvertedEntries = g.Count(wl => wl.ConvertedToAllotment),
                    CancelledEntries = g.Count(wl => wl.Status == "Cancelled"),
                    ExpiredEntries = g.Count(wl => wl.Status == "Expired"),
                    ActiveEntries = g.Count(wl => wl.IsActive()),
                    ConversionRate = g.Count() > 0 ? (decimal)g.Count(wl => wl.ConvertedToAllotment) / g.Count() * 100 : 0,
                    AverageWaitingDays = g.Average(wl => (DateTime.UtcNow - wl.EntryDate).TotalDays)
                })
                .OrderBy(m => m.Month)
                .ToList();

            return report;
        }
    }
}
