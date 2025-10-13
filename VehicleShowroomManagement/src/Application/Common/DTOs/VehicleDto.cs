namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for Vehicle entities (new schema)
    /// </summary>
    public class VehicleDto
    {
        public string VehicleId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string? ExternalNumber { get; set; }
        public string? RegistrationDataJson { get; set; }  // Serialized JSON string
        public string Status { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public DateTime ReceiptDate { get; set; }
    }
}
