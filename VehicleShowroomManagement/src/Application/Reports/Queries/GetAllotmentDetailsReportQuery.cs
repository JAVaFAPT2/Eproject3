using MediatR;
using VehicleShowroomManagement.Application.Reports.DTOs;

namespace VehicleShowroomManagement.Application.Reports.Queries
{
    /// <summary>
    /// Query to get allotment details report
    /// </summary>
    public class GetAllotmentDetailsReportQuery : IRequest<AllotmentDetailsReportDto>
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? AllotmentType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IncludeExpired { get; set; } = true;
        public bool IncludeConverted { get; set; } = true;
    }
}
