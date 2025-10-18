namespace VehicleShowroomManagement.Application.Features.VehicleModels.Queries.GetVehicleModelBySlug
{
    public class GetVehicleModelBySlugQueryHandler(IRepository<VehicleModel> modelRepository)
        : IRequestHandler<GetVehicleModelBySlugQuery, VehicleModel?>
    {
        public async Task<VehicleModel?> Handle(GetVehicleModelBySlugQuery request, CancellationToken cancellationToken)
        {
            var models = await modelRepository.FindAsync(m => m.Slug == request.Slug && m.DeletedAt == null, cancellationToken);
            return models.FirstOrDefault();
        }
    }
}


