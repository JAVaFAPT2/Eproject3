using MediatR;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Handlers
{
    /// <summary>
    /// Handler for approving purchase orders
    /// </summary>
    public class ApprovePurchaseOrderCommandHandler : IRequestHandler<ApprovePurchaseOrderCommand, bool>
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApprovePurchaseOrderCommandHandler(
            IRepository<PurchaseOrder> purchaseOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ApprovePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(request.Id);
            if (purchaseOrder == null)
                return false;

            if (purchaseOrder.Status != "Submitted")
                return false;

            purchaseOrder.ApproveOrder();

            await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
