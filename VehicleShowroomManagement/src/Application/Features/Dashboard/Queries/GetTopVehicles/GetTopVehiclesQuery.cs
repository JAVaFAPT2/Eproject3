using MediatR;

namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetTopVehicles
{
    /// <summary>
    /// Query for getting top selling vehicles
    /// </summary>
    public record GetTopVehiclesQuery(int Top, DateTime? FromDate, DateTime? ToDate) : IRequest<List<TopVehicleDto>>;

    /// <summary>
    /// Top vehicle DTO
    /// </summary>
    public class TopVehicleDto
    {
        public string VehicleId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int SalesCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AveragePrice { get; set; }
        public DateTime LastSaleDate { get; set; }
        public int AvailableStock { get; set; }
    }
}