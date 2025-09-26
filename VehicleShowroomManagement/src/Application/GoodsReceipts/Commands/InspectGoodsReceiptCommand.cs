using MediatR;

namespace VehicleShowroomManagement.Application.GoodsReceipts.Commands
{
    /// <summary>
    /// Command to inspect a goods receipt
    /// </summary>
    public class InspectGoodsReceiptCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string InspectedBy { get; set; } = string.Empty;
        public string? InspectionNotes { get; set; }
    }
}
