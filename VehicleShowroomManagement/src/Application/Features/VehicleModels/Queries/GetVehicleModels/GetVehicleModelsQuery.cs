namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModels
{
    public record GetVehicleModelsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Search = null) : IRequest<GetVehicleModelsResult>;

    public class GetVehicleModelsResult
    {
        public List<VehicleModelDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    public class VehicleModelDto
    {
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Level { get; set; }
        public string? ParentModel { get; set; }
        public string? Description { get; set; }
    }
}

