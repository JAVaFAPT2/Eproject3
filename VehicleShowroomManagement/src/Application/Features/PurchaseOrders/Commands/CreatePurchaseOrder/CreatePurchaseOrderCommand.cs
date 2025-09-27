using MediatR;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder
{
    public class CreatePurchaseOrderCommand : IRequest<string>
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? Notes { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}