using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.UpdatePurchaseOrderStatus
{
    public class UpdatePurchaseOrderStatusCommandHandler(IRepository<PurchaseOrder> poRepository)
        : IRequestHandler<UpdatePurchaseOrderStatusCommand, Unit>
    {
        public async Task<Unit> Handle(UpdatePurchaseOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var po = await poRepository.GetByIdAsync(request.PurchaseOrderId, cancellationToken);
            if (po == null) return Unit.Value;
            po.UpdateStatus(request.Status);
            await poRepository.UpdateAsync(po, cancellationToken);
            return Unit.Value;
        }
    }
}


