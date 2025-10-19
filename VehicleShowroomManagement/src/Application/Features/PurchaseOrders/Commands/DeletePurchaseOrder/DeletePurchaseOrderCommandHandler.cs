using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder
{
    /// <summary>
    /// Handler for deleting a purchase order and all its lines
    /// </summary>
    public class DeletePurchaseOrderCommandHandler(
        IRepository<PurchaseOrder> purchaseOrderRepository,
        IRepository<PurchaseOrderLine> purchaseOrderLineRepository) : IRequestHandler<DeletePurchaseOrderCommand>
    {
        public async Task Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            // First delete all lines for this purchase order
            var lines = await purchaseOrderLineRepository.FindAsync(line => line.POId == request.Id, cancellationToken);
            foreach (var line in lines)
            {
                await purchaseOrderLineRepository.DeleteAsync(line, cancellationToken);
            }

            // Then delete the purchase order itself
            var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(request.Id, cancellationToken);
            if (purchaseOrder != null)
            {
                await purchaseOrderRepository.DeleteAsync(purchaseOrder, cancellationToken);
            }
        }
    }
}
