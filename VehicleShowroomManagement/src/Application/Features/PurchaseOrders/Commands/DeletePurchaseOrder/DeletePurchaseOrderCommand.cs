namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.DeletePurchaseOrder
{
    /// <summary>
    /// Command for deleting a purchase order and all its lines
    /// </summary>
    public record DeletePurchaseOrderCommand(string Id) : IRequest;
}
