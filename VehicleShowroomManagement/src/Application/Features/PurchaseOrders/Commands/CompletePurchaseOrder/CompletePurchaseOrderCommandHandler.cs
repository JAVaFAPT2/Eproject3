using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CompletePurchaseOrder
{
    /// <summary>
    /// Completes a purchase order and creates vehicles from purchase order lines
    /// </summary>
    public class CompletePurchaseOrderCommandHandler : IRequestHandler<CompletePurchaseOrderCommand, List<string>>
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<PurchaseOrderLine> _purchaseOrderLineRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public CompletePurchaseOrderCommandHandler(
            IRepository<PurchaseOrder> purchaseOrderRepository,
            IRepository<PurchaseOrderLine> purchaseOrderLineRepository,
            IRepository<Vehicle> vehicleRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderLineRepository = purchaseOrderLineRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<List<string>> Handle(CompletePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            // Get purchase order
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                throw new InvalidOperationException("Purchase order not found");
            }

            if (!purchaseOrder.CanBeCompleted)
            {
                throw new InvalidOperationException("Purchase order cannot be completed");
            }

            // Get all purchase order lines
            var poLines = await _purchaseOrderLineRepository.FindAsync(pol => pol.POId == request.PurchaseOrderId);

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

                    await _vehicleRepository.AddAsync(vehicle);
                    createdVehicleIds.Add(vehicleId);
                }
            }

            // Mark purchase order as completed
            purchaseOrder.Complete();
            await _purchaseOrderRepository.UpdateAsync(purchaseOrder);

            return createdVehicleIds;
        }
    }
}
