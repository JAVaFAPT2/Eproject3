using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicle
{
    /// <summary>
    /// Handler for update vehicle command
    /// </summary>
    public class UpdateVehicleCommandHandler(IRepository<Vehicle> vehicleRepository) : IRequestHandler<UpdateVehicleCommand>
    {
        public async Task Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await vehicleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (vehicle == null)
                throw new ArgumentException("Vehicle not found");

            // Update vehicle properties using domain methods where available
            if (!string.IsNullOrEmpty(request.ExternalNumber))
                vehicle.UpdateExternalNumber(request.ExternalNumber);
            
            if (!string.IsNullOrEmpty(request.Vin))
                vehicle.SetVin(request.Vin);
            
            if (!string.IsNullOrEmpty(request.LicensePlate))
                vehicle.SetLicensePlate(request.LicensePlate);
            
            // Update purchase price and model number using reflection (no domain methods available)
            // Note: These should ideally have domain methods for validation
            var vehicleType = typeof(Vehicle);
            var purchasePriceProperty = vehicleType.GetProperty("PurchasePrice", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var modelNumberProperty = vehicleType.GetProperty("ModelNumber", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            
            if (purchasePriceProperty != null && purchasePriceProperty.CanWrite)
            {
                var backingField = vehicleType.GetField($"<{purchasePriceProperty.Name}>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                backingField?.SetValue(vehicle, request.PurchasePrice);
            }
            
            if (modelNumberProperty != null && modelNumberProperty.CanWrite)
            {
                var backingField = vehicleType.GetField($"<{modelNumberProperty.Name}>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                backingField?.SetValue(vehicle, request.ModelNumber);
            }

            await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        }
    }
}