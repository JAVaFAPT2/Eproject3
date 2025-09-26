using MediatR;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Commands
{
    /// <summary>
    /// Command to reject a goods receipt
    /// </summary>
    public class RejectGoodsReceiptCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }
}
