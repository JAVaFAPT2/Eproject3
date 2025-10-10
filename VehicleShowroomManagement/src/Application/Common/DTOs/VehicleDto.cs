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

    /// <summary>
    /// Data Transfer Object for Vehicle Photos
    /// </summary>
    public class VehiclePhotoDto
    {
        public string Id { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string? Caption { get; set; }
    }

    /// <summary>
    /// Data Transfer Object for Vehicle Specs
    /// </summary>
    public class VehicleSpecDto
    {
        public string Id { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string SpecName { get; set; } = string.Empty;
        public string SpecValue { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public string? GroupName { get; set; }
    }
}
