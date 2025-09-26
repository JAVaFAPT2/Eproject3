using MediatR;

namespace VehicleShowroomManagement.Application.Reports.Queries
{
    /// <summary>
    /// Query to get vehicle master information report
    /// </summary>
    public class GetVehicleMasterReportQuery : IRequest<VehicleMasterReportDto>
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Status { get; set; }
        public int? Year { get; set; }
        public string? Color { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IncludeRegistrationInfo { get; set; } = true;
        public bool IncludeServiceHistory { get; set; } = true;
    }
}
