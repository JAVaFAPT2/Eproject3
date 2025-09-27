using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.SearchVehicles
{
    /// <summary>
    /// Query to search vehicles with filters - critical for showroom operations
    /// </summary>
    public record SearchVehiclesQuery : IRequest<SearchVehiclesResult>
    {
        public string? SearchTerm { get; init; }
        public VehicleStatus? Status { get; init; }
        public string? ModelNumber { get; init; }
        public string? Brand { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public SearchVehiclesQuery(
            string? searchTerm = null,
            VehicleStatus? status = null,
            string? modelNumber = null,
            string? brand = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            SearchTerm = searchTerm;
            Status = status;
            ModelNumber = modelNumber;
            Brand = brand;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// Search results with pagination
    /// </summary>
    public class SearchVehiclesResult
    {
        public List<VehicleSearchDto> Vehicles { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }

    /// <summary>
    /// Lightweight vehicle DTO for search results
    /// </summary>
    public class VehicleSearchDto
    {
        public string Id { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string? Vin { get; set; }
        public VehicleStatus Status { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime ReceiptDate { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsRegistered { get; set; }
    }
}