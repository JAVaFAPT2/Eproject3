using MediatR;
using VehicleShowroomManagement.Application.Reports.DTOs;

namespace VehicleShowroomManagement.Application.Reports.Queries
{
    /// <summary>
    /// Query to get customer information report
    /// </summary>
    public class GetCustomerInfoReportQuery : IRequest<CustomerInfoReportDto>
    {
        public string? SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public bool IncludeOrderHistory { get; set; } = true;
    }
}
