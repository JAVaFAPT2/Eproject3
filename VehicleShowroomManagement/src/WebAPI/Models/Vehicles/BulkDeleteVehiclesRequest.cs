namespace VehicleShowroomManagement.WebAPI.Models.Vehicles
{
    /// <summary>
    /// Request model for bulk deleting vehicles
    /// </summary>
    public class BulkDeleteVehiclesRequest
    {
        public List<string> VehicleIds { get; set; } = new List<string>();
    }
}