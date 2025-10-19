using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrderLines.Commands.DeletePurchaseOrderLine
{
    /// <summary>
    /// Handler for deleting a single purchase order line
    /// </summary>
    public class DeletePurchaseOrderLineCommandHandler(
        IRepository<PurchaseOrderLine> purchaseOrderLineRepository,
        IRepository<PurchaseOrder> purchaseOrderRepository) : IRequestHandler<DeletePurchaseOrderLineCommand>
    {
        public async Task Handle(DeletePurchaseOrderLineCommand request, CancellationToken cancellationToken)
        {
            var line = await purchaseOrderLineRepository.GetByIdAsync(request.LineId, cancellationToken);
            if (line == null)
                throw new ArgumentException("Purchase order line not found");

            var poId = line.POId;

            // Delete the line
            await purchaseOrderLineRepository.DeleteAsync(line, cancellationToken);

            // Recalculate and update the purchase order total
            var remainingLines = await purchaseOrderLineRepository.FindAsync(l => l.POId == poId, cancellationToken);
            var newTotal = remainingLines.Sum(l => l.LineTotal);

            var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(poId, cancellationToken);
            if (purchaseOrder != null)
            {
                purchaseOrder.UpdateTotalAmount(newTotal);
                await purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);
            }
        }
    }
}
