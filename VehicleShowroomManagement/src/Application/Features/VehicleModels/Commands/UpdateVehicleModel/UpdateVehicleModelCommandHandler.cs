namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.UpdateVehicleModel
{
    /// <summary>
    /// Handler for updating vehicle model
    /// </summary>
    public class UpdateVehicleModelCommandHandler(IRepository<VehicleModel> modelRepository) : IRequestHandler<UpdateVehicleModelCommand>
    {
        private readonly IRepository<VehicleModel> _modelRepository = modelRepository;

        public async Task Handle(UpdateVehicleModelCommand request, CancellationToken cancellationToken)
        {
            // Fetch existing vehicle model
            var vehicleModel = await _modelRepository.GetByIdAsync(request.ModelNumber, cancellationToken);
            if (vehicleModel == null)
            {
                throw new InvalidOperationException("Vehicle model not found");
            }

            // Update using domain method
            vehicleModel.UpdateModel(
                request.Name,
                request.Brand,
                request.Price,
                request.Description,
                request.ImageUrl);

            await _modelRepository.UpdateAsync(vehicleModel, cancellationToken);
        }
    }
}

