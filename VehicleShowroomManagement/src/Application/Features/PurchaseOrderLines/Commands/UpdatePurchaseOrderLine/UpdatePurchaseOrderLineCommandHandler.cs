using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrderLines.Commands.UpdatePurchaseOrderLine
{
    /// <summary>
    /// Handler for updating a purchase order line
    /// </summary>
    public class UpdatePurchaseOrderLineCommandHandler(
        IRepository<PurchaseOrderLine> purchaseOrderLineRepository,
        IRepository<PurchaseOrder> purchaseOrderRepository) : IRequestHandler<UpdatePurchaseOrderLineCommand>
    {
        public async Task Handle(UpdatePurchaseOrderLineCommand request, CancellationToken cancellationToken)
        {
            var line = await purchaseOrderLineRepository.GetByIdAsync(request.LineId, cancellationToken);
            if (line == null)
                throw new ArgumentException("Purchase order line not found");

            var poId = line.POId;

            // Update line properties
            if (request.Quantity.HasValue)
                line.UpdateQuantity(request.Quantity.Value);

            if (request.PricePerUnit.HasValue)
                line.UpdatePricePerUnit(request.PricePerUnit.Value);

            // Save the updated line
            await purchaseOrderLineRepository.UpdateAsync(line, cancellationToken);

            // Recalculate and update the purchase order total
            var allLines = await purchaseOrderLineRepository.FindAsync(l => l.POId == poId, cancellationToken);
            var newTotal = allLines.Sum(l => l.LineTotal);

            var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(poId, cancellationToken);
            if (purchaseOrder != null)
            {
                purchaseOrder.UpdateTotalAmount(newTotal);
                await purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);
            }
        }
    }
}
