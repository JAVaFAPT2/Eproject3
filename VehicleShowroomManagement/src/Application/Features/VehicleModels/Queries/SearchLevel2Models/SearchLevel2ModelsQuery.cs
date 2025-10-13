namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.SearchLevel2Models
{
    public record SearchLevel2ModelsQuery(
        string? ParentModelNumber,
        int? Seats,
        string? FuelType,
        int PageNumber = 1,
        int PageSize = 10) : IRequest<List<VehicleModel>>;
}


