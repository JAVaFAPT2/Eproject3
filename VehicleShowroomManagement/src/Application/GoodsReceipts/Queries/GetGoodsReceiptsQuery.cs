using MediatR;
using VehicleShowroomManagement.Application.GoodsReceipts.DTOs;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Queries
{
    /// <summary>
    /// Query to get all goods receipts with filtering and pagination
    /// </summary>
    public class GetGoodsReceiptsQuery : IRequest<IEnumerable<GoodsReceiptDto>>
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? Brand { get; set; }
        public string? Condition { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
