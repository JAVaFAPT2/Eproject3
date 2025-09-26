using MediatR;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Commands
{
    /// <summary>
    /// Command to approve a purchase order
    /// </summary>
    public class ApprovePurchaseOrderCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}
