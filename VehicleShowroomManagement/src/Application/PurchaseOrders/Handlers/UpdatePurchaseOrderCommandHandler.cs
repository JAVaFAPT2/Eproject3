using MediatR;
using VehicleShowroomManagement.Application.PurchaseOrders.Commands;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Handlers
{
    /// <summary>
    /// Handler for updating purchase orders
    /// </summary>
    public class UpdatePurchaseOrderCommandHandler : IRequestHandler<UpdatePurchaseOrderCommand, bool>
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePurchaseOrderCommandHandler(
            IRepository<PurchaseOrder> purchaseOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(request.Id);
            if (purchaseOrder == null)
                return false;

            if (!purchaseOrder.CanBeModified())
                return false;

            purchaseOrder.ModelNumber = request.ModelNumber;
            purchaseOrder.Name = request.Name;
            purchaseOrder.Brand = request.Brand;
            purchaseOrder.Price = request.Price;
            purchaseOrder.Quantity = request.Quantity;
            purchaseOrder.ExpectedDeliveryDate = request.ExpectedDeliveryDate;
            purchaseOrder.SupplierId = request.SupplierId;
            purchaseOrder.SupplierName = request.SupplierName;
            purchaseOrder.Notes = request.Notes;

            purchaseOrder.CalculateTotalAmount();

            await _purchaseOrderRepository.UpdateAsync(purchaseOrder);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
