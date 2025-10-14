using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicleById
{
    /// <summary>
    /// Handler for getting a vehicle by ID
    /// </summary>
    public class GetVehicleByIdQueryHandler(IRepository<Vehicle> vehicleRepository) : IRequestHandler<GetVehicleByIdQuery, VehicleDto?>
    {

        public async Task<VehicleDto?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await vehicleRepository.FindAsync(v => v.VehicleId == request.VehicleId, cancellationToken);
            var vehicle = vehicles.FirstOrDefault();

            if (vehicle is null)
                return null;

            return new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                ModelNumber = vehicle.ModelNumber,
                ExternalNumber = vehicle.ExternalNumber,
                RegistrationDataJson = null,  // TODO: Implement BsonDocument to JSON serialization
                Status = vehicle.Status.ToString(),
                PurchasePrice = vehicle.PurchasePrice,
            };
        }
    }
}
