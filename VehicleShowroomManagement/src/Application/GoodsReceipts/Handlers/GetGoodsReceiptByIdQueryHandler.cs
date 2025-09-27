using MediatR;
using VehicleShowroomManagement.Application.GoodsReceipts.DTOs;
using VehicleShowroomManagement.Application.GoodsReceipts.Queries;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Handlers
{
    /// <summary>
    /// Handler for getting goods receipt by ID
    /// </summary>
    public class GetGoodsReceiptByIdQueryHandler : IRequestHandler<GetGoodsReceiptByIdQuery, GoodsReceiptDto?>
    {
        private readonly IRepository<GoodsReceipt> _goodsReceiptRepository;

        public GetGoodsReceiptByIdQueryHandler(IRepository<GoodsReceipt> goodsReceiptRepository)
        {
            _goodsReceiptRepository = goodsReceiptRepository;
        }

        public async Task<GoodsReceiptDto?> Handle(GetGoodsReceiptByIdQuery request, CancellationToken cancellationToken)
        {
            var goodsReceipt = await _goodsReceiptRepository.GetByIdAsync(request.Id);
            if (goodsReceipt == null)
                return null;

            return new GoodsReceiptDto
            {
                Id = goodsReceipt.Id,
                ReceiptNumber = goodsReceipt.ReceiptNumber,
                PurchaseOrderId = goodsReceipt.PurchaseOrderId,
                VehicleId = goodsReceipt.VehicleId,
                VIN = goodsReceipt.VIN,
                ExternalId = goodsReceipt.ExternalId,
                ModelNumber = goodsReceipt.ModelNumber,
                Name = goodsReceipt.Name,
                Brand = goodsReceipt.Brand,
                Price = goodsReceipt.Price,
                Quantity = goodsReceipt.Quantity,
                Status = goodsReceipt.Status,
                ReceiptDate = goodsReceipt.ReceiptDate,
                InspectedDate = goodsReceipt.InspectedDate,
                InspectedBy = goodsReceipt.InspectedBy,
                InspectionNotes = goodsReceipt.InspectionNotes,
                Condition = goodsReceipt.Condition,
                Mileage = goodsReceipt.Mileage,
                Color = goodsReceipt.Color,
                Year = goodsReceipt.Year,
                SupplierId = goodsReceipt.SupplierId,
                SupplierName = goodsReceipt.SupplierName,
                DeliveryNote = goodsReceipt.DeliveryNote,
                ReceivedBy = goodsReceipt.ReceivedBy,
                CreatedAt = goodsReceipt.CreatedAt,
                UpdatedAt = goodsReceipt.UpdatedAt
            };
        }
    }
}
