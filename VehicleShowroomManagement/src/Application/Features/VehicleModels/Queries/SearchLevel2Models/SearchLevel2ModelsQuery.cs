namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.SearchLevel2Models
{
    public record SearchLevel2ModelsQuery(
        string? ParentModelNumber,
        int? Seats,
        string? FuelType,
        int PageNumber = 1,
        int PageSize = 10) : IRequest<SearchLevel2ModelsResult>;

    public class SearchLevel2ModelsResult
    {
        public List<VehicleModel> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}


