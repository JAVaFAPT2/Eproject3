using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Queries.GetPurchaseOrderById
{
    /// <summary>
    /// Handler for getting a single purchase order by ID with lines included
    /// </summary>
    public class GetPurchaseOrderByIdQueryHandler(
        IRepository<PurchaseOrder> purchaseOrderRepository,
        IRepository<PurchaseOrderLine> purchaseOrderLineRepository) : IRequestHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDetailDto?>
    {
        public async Task<PurchaseOrderDetailDto?> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(request.Id, cancellationToken);
            if (purchaseOrder == null)
                return null;

            // Get all lines for this purchase order
            var lines = await purchaseOrderLineRepository.FindAsync(line => line.POId == request.Id, cancellationToken);

            return new PurchaseOrderDetailDto
            {
                Id = purchaseOrder.Id,
                CreatedBy = purchaseOrder.CreatedBy,
                OrderDate = purchaseOrder.OrderDate,
                TotalAmount = purchaseOrder.TotalAmount,
                Status = (int)purchaseOrder.Status,
                Lines = lines.Select(line => new PurchaseOrderLineDto
                {
                    Id = line.Id,
                    ModelId = line.ModelId,
                    Quantity = line.Quantity,
                    PricePerUnit = line.PricePerUnit,
                    LineTotal = line.LineTotal
                }).ToList()
            };
        }
    }
}
