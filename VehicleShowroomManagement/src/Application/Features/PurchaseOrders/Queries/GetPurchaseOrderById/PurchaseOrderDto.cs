using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById
{
    public class PurchaseOrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public string? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? Notes { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static PurchaseOrderDto FromEntity(PurchaseOrder purchaseOrder)
        {
            return new PurchaseOrderDto
            {
                Id = purchaseOrder.Id,
                OrderNumber = purchaseOrder.OrderNumber,
                ModelNumber = purchaseOrder.ModelNumber,
                Name = purchaseOrder.Name,
                Brand = purchaseOrder.Brand,
                Price = purchaseOrder.Price,
                Quantity = purchaseOrder.Quantity,
                TotalAmount = purchaseOrder.TotalAmount,
                Status = purchaseOrder.Status,
                OrderDate = purchaseOrder.OrderDate,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate,
                ActualDeliveryDate = purchaseOrder.ActualDeliveryDate,
                SupplierId = purchaseOrder.SupplierId,
                SupplierName = purchaseOrder.SupplierName,
                Notes = purchaseOrder.Notes,
                CreatedBy = purchaseOrder.CreatedBy,
                CreatedAt = purchaseOrder.CreatedAt,
                UpdatedAt = purchaseOrder.UpdatedAt
            };
        }
    }
}