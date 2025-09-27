using MediatR;

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
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);

            if (vehicle == null || vehicle.IsDeleted)
                return null;

            return new VehicleDto
            {
                Id = vehicle.Id,
                VehicleId = vehicle.VehicleId,
                ModelNumber = vehicle.ModelNumber,
                ExternalNumber = vehicle.ExternalNumber,
                Vin = vehicle.Vin,
                LicensePlate = vehicle.LicensePlate,
                RegistrationDate = vehicle.RegistrationDate,
                ExpiryDate = vehicle.ExpiryDate,
                Status = vehicle.Status,
                PurchasePrice = vehicle.PurchasePrice,
                SalePrice = vehicle.SalePrice,
                Photos = vehicle.Photos,
                ReceiptDate = vehicle.ReceiptDate,
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt,
                IsAvailable = vehicle.IsAvailable,
                IsSold = vehicle.IsSold,
                IsReserved = vehicle.IsReserved,
                IsRegistered = vehicle.IsRegistered
            };
        }
    }
}