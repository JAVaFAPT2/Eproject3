using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;
using VehicleShowroomManagement.Domain.ValueObjects;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle
{
    /// <summary>
    /// Handler for creating a new vehicle
    /// </summary>
    public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, string>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateVehicleCommandHandler(
            IVehicleRepository vehicleRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<string> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            // Create VIN value object if provided
            Vin? vin = null;
            if (!string.IsNullOrWhiteSpace(request.Vin))
            {
                vin = new Vin(request.Vin);
            }

            // Create vehicle
            var vehicle = new Vehicle(
                request.VehicleId,
                request.ModelNumber,
                request.PurchasePrice,
                request.ExternalNumber,
                vin,
                request.LicensePlate,
                request.ReceiptDate);

            // Add to repository
            await _vehicleRepository.AddAsync(vehicle);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Publish domain event
            var vehicleCreatedEvent = new VehicleCreatedEvent(vehicle.Id, vehicle.ModelNumber, vehicle.PurchasePrice);
            await _mediator.Publish(vehicleCreatedEvent, cancellationToken);

            return vehicle.Id;
        }
    }
}