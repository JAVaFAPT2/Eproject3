namespace VehicleShowroomManagement.Application.Features.PurchaseOrderLines.Commands.DeletePurchaseOrderLine
{
    /// <summary>
    /// Command for deleting a single purchase order line
    /// </summary>
    public record DeletePurchaseOrderLineCommand(string LineId) : IRequest;
}
