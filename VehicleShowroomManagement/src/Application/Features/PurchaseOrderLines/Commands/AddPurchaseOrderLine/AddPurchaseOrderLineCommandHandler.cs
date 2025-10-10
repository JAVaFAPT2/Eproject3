using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrderLines.Commands.AddPurchaseOrderLine
{
    public class AddPurchaseOrderLineCommandHandler : IRequestHandler<AddPurchaseOrderLineCommand, string>
    {
        private readonly IRepository<PurchaseOrderLine> _purchaseOrderLineRepository;
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IRepository<VehicleModel> _vehicleModelRepository;

        public AddPurchaseOrderLineCommandHandler(
            IRepository<PurchaseOrderLine> purchaseOrderLineRepository,
            IRepository<PurchaseOrder> purchaseOrderRepository,
            IRepository<VehicleModel> vehicleModelRepository)
        {
            _purchaseOrderLineRepository = purchaseOrderLineRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _vehicleModelRepository = vehicleModelRepository;
        }

        public async Task<string> Handle(AddPurchaseOrderLineCommand request, CancellationToken cancellationToken)
        {
            // Verify purchase order exists
            var po = await _purchaseOrderRepository.GetByIdAsync(request.POId);
            if (po == null)
            {
                throw new InvalidOperationException("Purchase order not found");
            }

            // Verify vehicle model exists
            var models = await _vehicleModelRepository.FindAsync(vm => vm.ModelNumber == request.ModelNumber);
            if (!models.Any())
            {
                throw new InvalidOperationException("Vehicle model not found");
            }

            var line = new PurchaseOrderLine(
                request.POId,
                request.ModelNumber,
                request.Quantity,
                request.PricePerUnit);

            await _purchaseOrderLineRepository.AddAsync(line);

            // Update PO total amount
            var allLines = await _purchaseOrderLineRepository.FindAsync(pol => pol.POId == request.POId);
            var newTotal = allLines.Sum(l => l.LineTotal);
            po.UpdateTotalAmount(newTotal);
            await _purchaseOrderRepository.UpdateAsync(po);

            return line.Id;
        }
    }
}

