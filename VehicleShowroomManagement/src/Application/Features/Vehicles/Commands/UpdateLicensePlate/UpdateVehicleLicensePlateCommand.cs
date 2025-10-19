namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateLicensePlate
{
    /// <summary>
    /// Command for updating vehicle license plate
    /// </summary>
    public record UpdateVehicleLicensePlateCommand(
        string VehicleId,
        string LicensePlate) : IRequest<bool>;
}
