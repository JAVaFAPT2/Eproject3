using MediatR;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Handlers
{
    /// <summary>
    /// Handler for creating goods receipts
    /// </summary>
    public class CreateGoodsReceiptCommandHandler : IRequestHandler<CreateGoodsReceiptCommand, string>
    {
        private readonly IRepository<GoodsReceipt> _goodsReceiptRepository;
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGoodsReceiptCommandHandler(
            IRepository<GoodsReceipt> goodsReceiptRepository,
            IRepository<PurchaseOrder> purchaseOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _goodsReceiptRepository = goodsReceiptRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateGoodsReceiptCommand request, CancellationToken cancellationToken)
        {
            // Verify purchase order exists and is approved
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId);
            if (purchaseOrder == null || purchaseOrder.Status != "Approved")
            {
                throw new InvalidOperationException("Purchase order not found or not approved");
            }

            var goodsReceipt = new GoodsReceipt
            {
                ReceiptNumber = GenerateReceiptNumber(),
                PurchaseOrderId = request.PurchaseOrderId,
                VehicleId = request.VehicleId,
                VIN = request.VIN,
                ExternalId = request.ExternalId,
                ModelNumber = request.ModelNumber,
                Name = request.Name,
                Brand = request.Brand,
                Price = request.Price,
                Quantity = request.Quantity,
                Condition = request.Condition,
                Mileage = request.Mileage,
                Color = request.Color,
                Year = request.Year,
                SupplierId = request.SupplierId,
                SupplierName = request.SupplierName,
                DeliveryNote = request.DeliveryNote,
                ReceivedBy = "System", // TODO: Get from current user context
                Status = "Received"
            };

            await _goodsReceiptRepository.AddAsync(goodsReceipt);
            await _unitOfWork.SaveChangesAsync();

            return goodsReceipt.Id;
        }

        private string GenerateReceiptNumber()
        {
            return $"GR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}
