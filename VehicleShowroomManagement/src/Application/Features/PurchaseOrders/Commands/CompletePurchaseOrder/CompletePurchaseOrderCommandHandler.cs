namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CompletePurchaseOrder
{
    /// <summary>
    /// Completes a purchase order and creates vehicles from purchase order lines
    /// </summary>
    public class CompletePurchaseOrderCommandHandler(
        IRepository<PurchaseOrder> purchaseOrderRepository,
        IRepository<PurchaseOrderLine> purchaseOrderLineRepository,
        IRepository<Vehicle> vehicleRepository) : IRequestHandler<CompletePurchaseOrderCommand, List<string>>
    {
        public async Task<List<string>> Handle(CompletePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            // Get purchase order
            var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId, cancellationToken) ?? throw new InvalidOperationException("Purchase order not found");

            if (!purchaseOrder.CanBeCompleted)
            {
                throw new InvalidOperationException("Purchase order cannot be completed");
            }

            // Get all purchase order lines
            var poLines = await purchaseOrderLineRepository.FindAsync(pol => pol.POId == request.PurchaseOrderId, cancellationToken);

            var createdVehicleIds = new List<string>();

            // Create vehicles for each line item
            foreach (var line in poLines)
            {
                for (int i = 0; i < line.Quantity; i++)
                {
                    var vehicleId = $"VEH-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}".ToUpper();
                    
                    var vehicle = new Vehicle(
                        vehicleId,
                        line.ModelNumber,
                        line.PricePerUnit,
                        null, // ExternalNumber
                        DateTime.UtcNow);

                    await vehicleRepository.AddAsync(vehicle, cancellationToken);
                    createdVehicleIds.Add(vehicleId);
                }
            }

            // Mark purchase order as completed
            purchaseOrder.Complete();
            await purchaseOrderRepository.UpdateAsync(purchaseOrder, cancellationToken);

            return createdVehicleIds;
        }
    }
}
