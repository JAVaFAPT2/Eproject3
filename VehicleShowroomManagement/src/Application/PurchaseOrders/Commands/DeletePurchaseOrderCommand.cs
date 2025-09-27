using MediatR;

namespace VehicleShowroomManagement.Application.PurchaseOrders.Commands
{
    /// <summary>
    /// Command to delete a purchase order
    /// </summary>
    public class DeletePurchaseOrderCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}
