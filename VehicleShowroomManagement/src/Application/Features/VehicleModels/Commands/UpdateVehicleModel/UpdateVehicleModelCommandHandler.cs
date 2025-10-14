namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.UpdateVehicleModel
{
    public class UpdateVehicleModelCommandHandler(IRepository<VehicleModel> modelRepository) : IRequestHandler<UpdateVehicleModelCommand>
    {
        public async Task Handle(UpdateVehicleModelCommand request, CancellationToken cancellationToken)
        {
            var model = await modelRepository.GetByIdAsync(request.ModelNumber, cancellationToken)
                ?? throw new KeyNotFoundException($"Vehicle model {request.ModelNumber} not found");

            model.UpdateModel(request.Name, request.Price, request.Description);
            model.SetHierarchy(request.ParentId, request.Level);
            model.SetSlug(request.Slug);
            model.SetPhoto(request.Photo);
            await modelRepository.UpdateAsync(model, cancellationToken);
        }
    }
}

