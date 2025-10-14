using MediatR;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrderLines.Commands.AddPurchaseOrderLine
{
    public record AddPurchaseOrderLineCommand(
        string POId,
        string ModelId,
        int Quantity,
        decimal PricePerUnit) : IRequest<string>;
}

