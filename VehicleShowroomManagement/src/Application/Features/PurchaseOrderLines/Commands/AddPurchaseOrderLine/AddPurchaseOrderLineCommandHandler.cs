
namespace VehicleShowroomManagement.Application.Features.PurchaseOrderLines.Commands.AddPurchaseOrderLine
{
    public class AddPurchaseOrderLineCommandHandler(
        IRepository<PurchaseOrderLine> purchaseOrderLineRepository,
        IRepository<PurchaseOrder> purchaseOrderRepository,
        IRepository<VehicleModel> vehicleModelRepository) : IRequestHandler<AddPurchaseOrderLineCommand, string>
    {
        public async Task<string> Handle(AddPurchaseOrderLineCommand request, CancellationToken cancellationToken)
        {
            // Verify purchase order exists
            var po = await purchaseOrderRepository.GetByIdAsync(request.POId, cancellationToken);
            if (po == null)
            {
                throw new InvalidOperationException("Purchase order not found");
            }

            // Verify vehicle model exists
            var models = await vehicleModelRepository.FindAsync(vm => vm.ModelNumber == request.ModelNumber, cancellationToken);
            if (!models.Any())
            {
                throw new InvalidOperationException("Vehicle model not found");
            }

            var line = new PurchaseOrderLine(
                request.POId,
                request.ModelNumber,
                request.Quantity,
                request.PricePerUnit);

            await purchaseOrderLineRepository.AddAsync(line, cancellationToken);

            // Update PO total amount
            var allLines = await purchaseOrderLineRepository.FindAsync(pol => pol.POId == request.POId, cancellationToken);
            var newTotal = allLines.Sum(l => l.LineTotal);
            po.UpdateTotalAmount(newTotal);
            await purchaseOrderRepository.UpdateAsync(po, cancellationToken);

            return line.Id;
        }
    }
}

