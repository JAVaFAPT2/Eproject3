namespace VehicleShowroomManagement.WebAPI.Models.Vehicles
{
    /// <summary>
    /// Request model for updating a vehicle
    /// </summary>
    public class UpdateVehicleRequest
    {
        public string ModelNumber { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public string? ExternalNumber { get; set; }
        public string? Vin { get; set; }
        public string? LicensePlate { get; set; }
        public string? Color { get; set; }
        public int Mileage { get; set; }
    }
}