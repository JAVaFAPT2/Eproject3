using MediatR;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Queries
{
    /// <summary>
    /// Query to get a goods receipt by ID
    /// </summary>
    public class GetGoodsReceiptByIdQuery : IRequest<GoodsReceiptDto?>
    {
        public string Id { get; set; } = string.Empty;
    }
}
