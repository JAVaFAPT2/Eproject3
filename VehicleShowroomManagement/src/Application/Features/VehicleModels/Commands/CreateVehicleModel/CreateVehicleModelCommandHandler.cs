using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.CreateVehicleModel
{
    public class CreateVehicleModelCommandHandler : IRequestHandler<CreateVehicleModelCommand, string>
    {
        private readonly IRepository<VehicleModel> _modelRepository;

        public CreateVehicleModelCommandHandler(IRepository<VehicleModel> modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<string> Handle(CreateVehicleModelCommand request, CancellationToken cancellationToken)
        {
            // Check if model number already exists
            var existing = await _modelRepository.FindAsync(vm => vm.ModelNumber == request.ModelNumber);
            if (existing.Any())
            {
                throw new InvalidOperationException("Model number already exists");
            }

            var vehicleModel = new VehicleModel(
                request.ModelNumber,
                request.Name,
                request.Brand,
                request.Price);

            await _modelRepository.AddAsync(vehicleModel);

            return vehicleModel.ModelNumber;
        }
    }
}
