using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Commands.UpdateOrderStatus
{
    /// <summary>
    /// Command for updating sales order status
    /// </summary>
    public record UpdateOrderStatusCommand(string OrderId, OrderStatus Status) : IRequest;
}