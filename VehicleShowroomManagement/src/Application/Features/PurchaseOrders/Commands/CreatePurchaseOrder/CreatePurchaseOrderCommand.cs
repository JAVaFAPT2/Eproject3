using MediatR;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CreatePurchaseOrder
{
    public record CreatePurchaseOrderCommand(
        string CreatedBy,
        decimal TotalAmount,
        DateTime? ExpectedDeliveryDate = null) : IRequest<string>;
}

