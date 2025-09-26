using MediatR;
using VehicleShowroomManagement.Application.PurchaseOrders.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Handlers
{
    /// <summary>
    /// Handler for getting purchase order by ID
    /// </summary>
    public class GetPurchaseOrderByIdQueryHandler : IRequestHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDto?>
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;

        public GetPurchaseOrderByIdQueryHandler(IRepository<PurchaseOrder> purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<PurchaseOrderDto?> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(request.Id);
            if (purchaseOrder == null)
                return null;

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
