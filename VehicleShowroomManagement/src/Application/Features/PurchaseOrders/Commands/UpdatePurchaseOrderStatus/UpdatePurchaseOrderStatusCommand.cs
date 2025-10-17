using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.PurchaseOrders.Commands.UpdatePurchaseOrderStatus
{
    public record UpdatePurchaseOrderStatusCommand(string PurchaseOrderId, PurchaseOrderStatus Status) : IRequest<Unit>;
}


