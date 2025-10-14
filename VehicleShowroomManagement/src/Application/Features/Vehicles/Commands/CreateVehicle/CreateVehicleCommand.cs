using MediatR;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle
{
    /// <summary>
    /// Command to create a new vehicle
    /// </summary>
    public record CreateVehicleCommand : IRequest<string>
    {
        public string VehicleId { get; init; }
        public string ModelNumber { get; init; }
        public decimal PurchasePrice { get; init; }
        public string? ExternalNumber { get; init; }
        public string? Vin { get; init; }

        public CreateVehicleCommand(
            string vehicleId,
            string modelNumber,
            decimal purchasePrice,
            string? externalNumber = null,
            string? vin = null)
        {
            VehicleId = vehicleId;
            ModelNumber = modelNumber;
            PurchasePrice = purchasePrice;
            ExternalNumber = externalNumber;
            Vin = vin;
        }
    }
}