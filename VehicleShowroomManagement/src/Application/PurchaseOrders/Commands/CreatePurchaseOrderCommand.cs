using MediatR;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Commands
{
    /// <summary>
    /// Command to create a new purchase order
    /// </summary>
    public class CreatePurchaseOrderCommand : IRequest<string>
    {
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? Notes { get; set; }
    }
}
