using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.UpdateVehicleModel
{
    /// <summary>
    /// Handler for updating vehicle model
    /// </summary>
    public class UpdateVehicleModelCommandHandler : IRequestHandler<UpdateVehicleModelCommand>
    {
        private readonly IRepository<VehicleModel> _modelRepository;

        public UpdateVehicleModelCommandHandler(IRepository<VehicleModel> modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task Handle(UpdateVehicleModelCommand request, CancellationToken cancellationToken)
        {
            // Fetch existing vehicle model
            var vehicleModel = await _modelRepository.GetByIdAsync(request.ModelNumber);
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

            await _modelRepository.UpdateAsync(vehicleModel);
        }
    }
}

