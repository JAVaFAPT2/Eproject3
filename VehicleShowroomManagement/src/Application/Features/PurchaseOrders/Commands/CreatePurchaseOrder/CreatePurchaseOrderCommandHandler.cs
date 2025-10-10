using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder
{
    public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, string>
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;

        public CreatePurchaseOrderCommandHandler(IRepository<PurchaseOrder> purchaseOrderRepository)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<string> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = new PurchaseOrder(
                request.CreatedBy,
                request.TotalAmount,
                request.ExpectedDeliveryDate);

            await _purchaseOrderRepository.AddAsync(purchaseOrder);

            return purchaseOrder.Id;
        }
    }
}

