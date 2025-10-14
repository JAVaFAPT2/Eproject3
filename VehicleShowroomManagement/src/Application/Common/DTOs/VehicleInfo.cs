namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Vehicle information for orders
    /// </summary>
    public class VehicleInfo
    {
        public string VehicleId { get; set; } = string.Empty;
        public string Vin { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
