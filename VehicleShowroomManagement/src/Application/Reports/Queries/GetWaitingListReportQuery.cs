using MediatR;

namespace VehicleShowroomManagement.Application.Reports.Queries
{
    /// <summary>
    /// Query to get waiting list details report
    /// </summary>
    public class GetWaitingListReportQuery : IRequest<WaitingListReportDto>
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? VehicleType { get; set; }
        public int? Priority { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IncludeExpired { get; set; } = true;
        public bool IncludeConverted { get; set; } = true;
    }
}
