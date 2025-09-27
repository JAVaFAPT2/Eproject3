using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Models.Vehicles
{
    /// <summary>
    /// Request model for updating vehicle status
    /// </summary>
    public class UpdateVehicleStatusRequest
    {
        public VehicleStatus Status { get; set; }
    }
}