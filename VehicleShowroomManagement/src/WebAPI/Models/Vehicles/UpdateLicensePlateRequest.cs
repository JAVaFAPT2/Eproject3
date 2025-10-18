namespace VehicleShowroomManagement.WebAPI.Models.Vehicles
{
    /// <summary>
    /// Request model for updating vehicle license plate
    /// </summary>
    public class UpdateLicensePlateRequest
    {
        public string LicensePlate { get; set; } = string.Empty;
    }
}
