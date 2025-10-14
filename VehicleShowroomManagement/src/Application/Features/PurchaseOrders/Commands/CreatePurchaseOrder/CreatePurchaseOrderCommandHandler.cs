namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder
{
    public class CreatePurchaseOrderCommandHandler(IRepository<PurchaseOrder> purchaseOrderRepository) : IRequestHandler<CreatePurchaseOrderCommand, string>
    {

        public async Task<string> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = new PurchaseOrder(
                request.CreatedBy,
                request.TotalAmount);

            await purchaseOrderRepository.AddAsync(purchaseOrder, cancellationToken);

            return purchaseOrder.Id;
        }
    }
}

