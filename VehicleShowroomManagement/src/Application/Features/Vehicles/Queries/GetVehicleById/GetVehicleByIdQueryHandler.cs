using MediatR;
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
            var vehicle = await vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);

            if (vehicle == null || vehicle.IsDeleted)
                return null;

            return new VehicleDto
            {
                Id = vehicle.Id,
                VehicleId = vehicle.VehicleId,
                VIN = vehicle.Vin ?? string.Empty,
                ModelNumber = vehicle.ModelNumber,
                ExternalNumber = vehicle.ExternalNumber,
                Name = string.Empty, // Not available in entity
                Brand = string.Empty, // Not available in entity
                BrandId = string.Empty, // Not available in entity
                ModelId = string.Empty, // Not available in entity
                Color = string.Empty, // Not available in entity
                Year = 0, // Not available in entity
                PurchasePrice = vehicle.PurchasePrice,
                Price = vehicle.SalePrice ?? vehicle.PurchasePrice, // Backward compatibility
                Mileage = 0, // Not available in entity
                Status = vehicle.Status.ToString(),
                LicensePlate = vehicle.LicensePlate,
                RegistrationNumber = string.Empty, // Not available in entity
                RegistrationDate = vehicle.RegistrationDate,
                ExpiryDate = vehicle.ExpiryDate,
                ExternalId = string.Empty, // Not available in entity
                Photos = vehicle.Photos,
                ReceiptDate = vehicle.ReceiptDate,
                CreatedAt = vehicle.CreatedAt,
                UpdatedAt = vehicle.UpdatedAt,
                Images = new List<VehicleImageDto>() // Not available in entity
            };
        }
    }
}