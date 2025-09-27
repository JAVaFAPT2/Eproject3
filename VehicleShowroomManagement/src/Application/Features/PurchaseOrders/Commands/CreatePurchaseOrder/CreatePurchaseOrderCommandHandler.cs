using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder
{
    public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, string>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePurchaseOrderCommandHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            ISupplierRepository supplierRepository,
            IUnitOfWork unitOfWork)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _supplierRepository = supplierRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            // Validate supplier if provided
            if (!string.IsNullOrEmpty(request.SupplierId))
            {
                var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);
                if (supplier == null)
                    throw new ArgumentException("Supplier not found", nameof(request.SupplierId));
            }

            // Calculate total amount
            var totalAmount = request.Price * request.Quantity;

            // Create purchase order
            var purchaseOrder = new PurchaseOrder(
                request.OrderNumber,
                request.ModelNumber,
                request.Name,
                request.Brand,
                request.Price,
                request.Quantity,
                totalAmount,
                request.SupplierId,
                request.SupplierName,
                request.Notes,
                request.ExpectedDeliveryDate,
                request.CreatedBy
            );

            // Add domain events
            purchaseOrder.AddDomainEvent(new PurchaseOrderCreatedEvent(purchaseOrder.Id, purchaseOrder.OrderNumber));

            // Save to repository
            await _purchaseOrderRepository.AddAsync(purchaseOrder);
            await _unitOfWork.SaveChangesAsync();

            return purchaseOrder.Id;
        }
    }
}