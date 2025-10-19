namespace VehicleShowroomManagement.Application.Features.PurchaseOrderLines.Commands.UpdatePurchaseOrderLine
{
    /// <summary>
    /// Command for updating a purchase order line
    /// </summary>
    public record UpdatePurchaseOrderLineCommand(
        string LineId,
        int? Quantity,
        decimal? PricePerUnit) : IRequest;
}
