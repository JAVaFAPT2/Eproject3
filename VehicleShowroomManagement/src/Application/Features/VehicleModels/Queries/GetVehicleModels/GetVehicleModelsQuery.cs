using MediatR;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModels
{
    public record GetVehicleModelsQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Brand = null) : IRequest<GetVehicleModelsResult>;

    public class GetVehicleModelsResult
    {
        public List<VehicleModelDto> VehicleModels { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class VehicleModelDto
    {
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}

