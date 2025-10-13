namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelBySlug
{
    public record GetVehicleModelBySlugQuery(string Slug) : IRequest<VehicleModel?>;
}


