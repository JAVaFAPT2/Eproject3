using MediatR;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Commands
{
    /// <summary>
    /// Command to accept a goods receipt
    /// </summary>
    public class AcceptGoodsReceiptCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}
