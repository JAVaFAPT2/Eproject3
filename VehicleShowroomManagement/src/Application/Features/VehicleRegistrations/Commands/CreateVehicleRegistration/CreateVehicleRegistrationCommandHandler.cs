using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.VehicleRegistrations.Commands.CreateVehicleRegistration
{
    public class CreateVehicleRegistrationCommandHandler : IRequestHandler<CreateVehicleRegistrationCommand, string>
    {
        private readonly IVehicleRegistrationRepository _vehicleRegistrationRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVehicleRegistrationCommandHandler(
            IVehicleRegistrationRepository vehicleRegistrationRepository,
            IVehicleRepository vehicleRepository,
            IUnitOfWork unitOfWork)
        {
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateVehicleRegistrationCommand request, CancellationToken cancellationToken)
        {
            // Validate vehicle exists
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
                throw new ArgumentException("Vehicle not found", nameof(request.VehicleId));

            // Create vehicle registration
            var vehicleRegistration = new VehicleRegistration(
                request.VehicleId,
                request.VIN,
                request.RegistrationNumber,
                request.RegistrationDate,
                request.RegistrationAuthority,
                request.RegistrationState,
                request.RegistrationCity,
                request.OwnerName,
                request.OwnerAddress,
                request.OwnerPhone,
                request.OwnerEmail,
                request.VehicleType,
                request.FuelType,
                request.EngineNumber,
                request.ChassisNumber,
                request.ManufacturingYear,
                request.ManufacturingMonth,
                request.SeatingCapacity,
                request.GrossWeight,
                request.UnladenWeight,
                request.ExpiryDate,
                request.CreatedBy
            );

            // Add domain events
            vehicleRegistration.AddDomainEvent(new VehicleRegistrationCreatedEvent(vehicleRegistration.Id, vehicleRegistration.RegistrationNumber));

            // Save to repository
            await _vehicleRegistrationRepository.AddAsync(vehicleRegistration);
            await _unitOfWork.SaveChangesAsync();

            return vehicleRegistration.Id;
        }
    }
}