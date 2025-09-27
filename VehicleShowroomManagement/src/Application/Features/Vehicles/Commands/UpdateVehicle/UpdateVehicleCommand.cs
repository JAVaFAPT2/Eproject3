using MediatR;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicle
{
    /// <summary>
    /// Command for updating vehicle information
    /// </summary>
    public record UpdateVehicleCommand(
        string Id,
        string ModelNumber,
        decimal PurchasePrice,
        string? ExternalNumber,
        string? Vin,
        string? LicensePlate,
        string? Color,
        int Mileage) : IRequest;
}