using MediatR;
using VehicleShowroomManagement.Application.PurchaseOrders.Commands;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Handlers
{
    /// <summary>
    /// Handler for deleting purchase orders
    /// </summary>
    public class DeletePurchaseOrderCommandHandler : IRequestHandler<DeletePurchaseOrderCommand, bool>
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePurchaseOrderCommandHandler(
            IRepository<PurchaseOrder> purchaseOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(request.Id);
            if (purchaseOrder == null)
                return false;

            if (!purchaseOrder.CanBeCancelled())
                return false;

            purchaseOrder.SoftDelete();

            await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
            _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
