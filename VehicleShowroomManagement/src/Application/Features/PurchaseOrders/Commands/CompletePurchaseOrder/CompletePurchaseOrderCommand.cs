using MediatR;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.CompletePurchaseOrder
{
    public record CompletePurchaseOrderCommand(string PurchaseOrderId) : IRequest<List<string>>;
}

