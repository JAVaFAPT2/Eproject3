using MediatR;
using VehicleShowroomManagement.Application.PurchaseOrders.Commands;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Handlers
{
    /// <summary>
    /// Handler for creating purchase orders
    /// </summary>
    public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, string>
    {
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePurchaseOrderCommandHandler(
            IRepository<PurchaseOrder> purchaseOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = new PurchaseOrder
            {
                OrderNumber = GenerateOrderNumber(),
                ModelNumber = request.ModelNumber,
                Name = request.Name,
                Brand = request.Brand,
                Price = request.Price,
                Quantity = request.Quantity,
                ExpectedDeliveryDate = request.ExpectedDeliveryDate,
                SupplierId = request.SupplierId,
                SupplierName = request.SupplierName,
                Notes = request.Notes,
                CreatedBy = "System", // TODO: Get from current user context
                Status = "Draft"
            };

            purchaseOrder.CalculateTotalAmount();

            await _purchaseOrderRepository.AddAsync(purchaseOrder);
            await _unitOfWork.SaveChangesAsync();

            return purchaseOrder.Id;
        }

        private string GenerateOrderNumber()
        {
            return $"PO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}
