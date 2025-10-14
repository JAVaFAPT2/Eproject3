namespace VehicleShowroomManagement.WebAPI.Models.Vehicles
{
    /// <summary>
    /// Request model for creating a vehicle
    /// </summary>
    public class CreateVehicleRequest
    {
        public string VehicleId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public string? ExternalNumber { get; set; }
        public string? Vin { get; set; }
    }
}