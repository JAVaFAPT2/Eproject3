using MediatR;
using VehicleShowroomManagement.Application.GoodsReceipts.DTOs;
using VehicleShowroomManagement.Application.GoodsReceipts.Queries;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Handlers
{
    /// <summary>
    /// Handler for getting goods receipts
    /// </summary>
    public class GetGoodsReceiptsQueryHandler : IRequestHandler<GetGoodsReceiptsQuery, IEnumerable<GoodsReceiptDto>>
    {
        private readonly IRepository<GoodsReceipt> _goodsReceiptRepository;

        public GetGoodsReceiptsQueryHandler(IRepository<GoodsReceipt> goodsReceiptRepository)
        {
            _goodsReceiptRepository = goodsReceiptRepository;
        }

        public async Task<IEnumerable<GoodsReceiptDto>> Handle(GetGoodsReceiptsQuery request, CancellationToken cancellationToken)
        {
            var goodsReceipts = await _goodsReceiptRepository.GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                goodsReceipts = goodsReceipts.Where(gr => 
                    gr.ReceiptNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    gr.VIN.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    gr.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    gr.Brand.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    gr.ModelNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                goodsReceipts = goodsReceipts.Where(gr => gr.Status == request.Status);
            }

            if (!string.IsNullOrEmpty(request.Brand))
            {
                goodsReceipts = goodsReceipts.Where(gr => gr.Brand == request.Brand);
            }

            if (!string.IsNullOrEmpty(request.Condition))
            {
                goodsReceipts = goodsReceipts.Where(gr => gr.Condition == request.Condition);
            }

            if (request.FromDate.HasValue)
            {
                goodsReceipts = goodsReceipts.Where(gr => gr.ReceiptDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                goodsReceipts = goodsReceipts.Where(gr => gr.ReceiptDate <= request.ToDate.Value);
            }

            // Apply pagination
            goodsReceipts = goodsReceipts
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            return goodsReceipts.Select(MapToDto);
        }

        private static GoodsReceiptDto MapToDto(GoodsReceipt goodsReceipt)
        {
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
