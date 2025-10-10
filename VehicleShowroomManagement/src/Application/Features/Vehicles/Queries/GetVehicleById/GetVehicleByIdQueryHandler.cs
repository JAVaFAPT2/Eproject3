using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicleById
{
    /// <summary>
    /// Handler for getting a vehicle by ID
    /// </summary>
    public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public GetVehicleByIdQueryHandler(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<VehicleDto?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.FindAsync(v => v.VehicleId == request.VehicleId);
            var vehicle = vehicles.FirstOrDefault();

            if (vehicle == null)
                return null;

            return new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                ModelNumber = vehicle.ModelNumber,
                ExternalNumber = vehicle.ExternalNumber,
                RegistrationDataJson = null,  // TODO: Implement BsonDocument to JSON serialization
                Status = vehicle.Status.ToString(),
                PurchasePrice = vehicle.PurchasePrice,
                ReceiptDate = vehicle.ReceiptDate
            };
        }
    }
}
