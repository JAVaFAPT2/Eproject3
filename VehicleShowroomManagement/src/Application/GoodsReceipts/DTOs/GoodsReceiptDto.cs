namespace VehicleShowroomManagement.Application.GoodsReceipts.DTOs
{
    /// <summary>
    /// Data Transfer Object for Goods Receipt
    /// </summary>
    public class GoodsReceiptDto
    {
        public string Id { get; set; } = string.Empty;
        public string ReceiptNumber { get; set; } = string.Empty;
        public string PurchaseOrderId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public string? ExternalId { get; set; }
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ReceiptDate { get; set; }
        public DateTime? InspectedDate { get; set; }
        public string? InspectedBy { get; set; }
        public string? InspectionNotes { get; set; }
        public string Condition { get; set; } = string.Empty;
        public int Mileage { get; set; }
        public string? Color { get; set; }
        public int? Year { get; set; }
        public string? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? DeliveryNote { get; set; }
        public string ReceivedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
