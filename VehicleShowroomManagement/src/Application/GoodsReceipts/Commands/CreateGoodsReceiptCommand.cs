using MediatR;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Commands
{
    /// <summary>
    /// Command to create a new goods receipt
    /// </summary>
    public class CreateGoodsReceiptCommand : IRequest<string>
    {
        public string PurchaseOrderId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public string? ExternalId { get; set; }
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        public string Condition { get; set; } = "New";
        public int Mileage { get; set; } = 0;
        public string? Color { get; set; }
        public int? Year { get; set; }
        public string? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? DeliveryNote { get; set; }
    }
}
