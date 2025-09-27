using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicles
{
    /// <summary>
    /// Query for getting vehicles with pagination and filters
    /// </summary>
    public record GetVehiclesQuery(
        int PageNumber,
        int PageSize,
        VehicleStatus? Status,
        string? Brand) : IRequest<GetVehiclesResult>;

    /// <summary>
    /// Result for get vehicles query
    /// </summary>
    public class GetVehiclesResult
    {
        public List<VehicleDto> Vehicles { get; set; } = new List<VehicleDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// Vehicle DTO for get vehicles result
    /// </summary>
    public class VehicleDto
    {
        public string Id { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? LicensePlate { get; set; }
        public string? Vin { get; set; }
        public int Mileage { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}