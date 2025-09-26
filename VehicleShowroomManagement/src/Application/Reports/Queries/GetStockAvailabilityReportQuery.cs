using MediatR;

namespace VehicleShowroomManagement.Application.Reports.Queries
{
    /// <summary>
    /// Query to get stock availability report
    /// </summary>
    public class GetStockAvailabilityReportQuery : IRequest<StockAvailabilityReportDto>
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Status { get; set; }
        public DateTime? AsOfDate { get; set; }
    }
}
